using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using System.Text.RegularExpressions;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Tracks user session data for the cybersecurity chatbot
    /// </summary>
    class UserSession
    {
        public string UserName { get; set; }        // User's name
        public int QueriesAsked { get; set; }       // Number of queries made
        public DateTime SessionStart { get; } = DateTime.Now; // Session start time
    }

    /// <summary>
    /// Main program class for the South African Cybersecurity Chatbot
    /// </summary>
    class Program
    {
        // Knowledge base with SA-specific cybersecurity topics and responses
        // Information adapted from Pieterse (2021) on SA cyber threats
        private static readonly Dictionary<string, List<string>> Responses = new()
        {
            { "phishing", new List<string> {
                "Phishing in SA: Many attacks target mobile banking users, with a significant rise noted in recent years.",
                "Common scams include fake SARS eFiling emails - always verify URLs before clicking!",
                "Tip: Look for 'https://' and official domain names (e.g., .gov.za).",
                "Report suspicious emails to phishing@cybersec.gov.za."
            }},
            { "malware", new List<string> {
                "Malware cases in SA have increased, often spread via email attachments.",
                "Ransomware locks files - never pay the ransom, report it instead.",
                "Prevention: Update software regularly and use antivirus tools.",
                "Common SA threat: Fake banking apps - download only from official stores."
            }},
            { "wifi", new List<string> {
                "Public Wi-Fi in SA (e.g., cafes) is a hotspot for data theft.",
                "Use a VPN for encryption on unsecured networks.",
                "Avoid banking or sensitive logins on public Wi-Fi.",
                "Set your home Wi-Fi to WPA3 security with a strong password."
            }},
            { "social", new List<string> {
                "Social engineering scams are prevalent in SA, often via phone calls.",
                "Beware of 'vishing' - fake SARS or bank calls asking for OTPs.",
                "Never share personal info with unsolicited callers.",
                "Verify identities by calling official numbers directly."
            }},
            { "purpose", new List<string> {
                "I’m here to help South African citizens stay safe online by providing cybersecurity tips.",
                "I can educate you on threats like phishing, malware, and social engineering."
            }},
            { "topics", new List<string> {
                "You can ask me about phishing threats, malware protection, Wi-Fi safety, or social engineering scams.",
                "I can also share South African cyber statistics or general online safety tips."
            }}
        };

        private static UserSession CurrentSession { get; set; } // Current user session

        /// <summary>
        /// Program entry point
        /// </summary>
        static void Main()
        {
            InitializeConsole();
            PlayVoiceGreeting(); 



            ShowCyberWelcomeBanner();
            PlayCyberSound();
            CurrentSession = new UserSession { UserName = GetValidName() };
            StartChatSession();
        }

        #region Console Setup and UI
        /// <summary>
        /// Configures console appearance and settings
        /// </summary>
        private static void InitializeConsole()
        {
            Console.Title = "SA Cyber Shield - Dept. of Cybersecurity";
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
        }

        /// <summary>
        /// Plays a recorded voice greeting at the start of the application
        /// </summary>
        private static void PlayVoiceGreeting()
        {
            try
            {
                // Play the WAV file for the voice greeting
                // File should contain: "Hello! Welcome to the Cybersecurity Awareness Bot. I’m here to help you stay safe online."
                new SoundPlayer("Audio/welcome.wav").PlaySync();
            }
            catch (Exception ex)
            {
                // Fallback if the audio file is missing or fails to play
                Console.WriteLine($"Voice greeting failed: {ex.Message}");
                Console.WriteLine("Text fallback: Hello! Welcome to the Cybersecurity Awareness Bot. I’m here to help you stay safe online.");
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Displays welcome banner with SA Cybersecurity Dept. branding
        /// </summary>
        private static void ShowCyberWelcomeBanner()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            ShowTypingEffect(@"
            ┌════════════════ SA CYBERSECURITY DEPT. ═══════════════┐
            │  ██████╗██╗   ██╗██████╗ ███████╗██████╗ ██╗   ██╗  │
            │ ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██║   ██║  │
            │ ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝██║   ██║  │
            │ ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗██║   ██║  │
            │ ╚██████╗   ██║   ██████╔╝███████╗██║  ██║╚██████╔╝  │
            │  ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝ ╚═════╝   │
            │ Protecting SA's Digital Future - Est. 2015           │
            └══════════════════════════════════════════════════════┘
            ", 10, ConsoleColor.Green);
            Console.ResetColor();
        }

        /// <summary>
        /// Displays exit banner with session stats and resources
        /// </summary>
        private static void ShowCyberExitBanner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            ShowTypingEffect(@"
            ┌═══════════════ SECURE SESSION TERMINATION ═════════════┐
            │  ███████╗██╗  ██╗██╗███████╗██╗     █████╗ ██╗   ██╗  │
            │  ██╔════╝██║  ██║██║██╔════╝██║    ██╔══██╗██║   ██║  │
            │  ███████╗███████║██║█████╗  ██║    ███████║██║   ██║  │
            │  ╚════██║██╔══██║██║██╔══╝  ██║    ██╔══██║██║   ██║  │
            │  ███████║██║  ██║██║███████╗██████╗██║  ██║╚██████╔╝  │
            │  ╚══════╝╚═╝  ╚═╝╚═╝╚══════╝╚═════╝╚═╝  ╚═╝ ╚═════╝   │
            └═══════════════════════════════════════════════════════┘
            ", 10, ConsoleColor.Green);
            ShowTypingEffect($"Session Stats for {CurrentSession.UserName}:", 30, ConsoleColor.White);
            ShowTypingEffect($"Duration: {(DateTime.Now - CurrentSession.SessionStart).Minutes} minutes", 30, ConsoleColor.White);
            ShowTypingEffect($"Queries: {CurrentSession.QueriesAsked}", 30, ConsoleColor.White);
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            ShowTypingEffect(@"
            ┌═══════════════ SA CYBERSECURITY RESOURCES ═════════════┐
            │ Emergency Contact: 0800-CYBER-SA (0800-29237-72)      │
            │ Report Incidents: www.cybersecurityhub.gov.za         │
            │ Email: info@cybersec.gov.za                           │
            │ Stay vigilant - Protect your digital identity!        │
            └═══════════════════════════════════════════════════════┘
            ", 20, ConsoleColor.Cyan);
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Displays the interactive menu with cybersecurity topics
        /// </summary>
        private static void DisplayEnhancedMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            ShowTypingEffect(@"
            ┌═════════════════ CYBER MENU ═════════════════┐
            │ 1. Phishing Threats                         │
            │ 2. Malware Protection                       │
            │ 3. Wi-Fi Safety                             │
            │ 4. Social Engineering Scams                 │
            │ 5. What’s Your Purpose?                     │
            │ 6. What Can I Ask About?                    │
            │ 7. SA Cyber Statistics                      │
            │ 8. Exit Securely                            │
            └═════════════════════════════════════════════┘
            ", 20, ConsoleColor.Yellow);
            Console.ResetColor();
            Console.Write("Your choice (1-8): ");
        }
        #endregion

        #region Chat Logic
        /// <summary>
        /// Manages the main chat loop and user interaction
        /// </summary>
        private static void StartChatSession()
        {
            ShowTypingEffect($"Greetings {CurrentSession.UserName}! Your Cyber Shield is active.", 30, ConsoleColor.White);
            
            while (true)
            {
                DisplayEnhancedMenu();
                string choice = GetValidatedChoice();

                switch (choice.ToLower())
                {
                    case "1":
                        HandleCyberQuery("phishing");
                        break;
                    case "2":
                        HandleCyberQuery("malware");
                        break;
                    case "3":
                        HandleCyberQuery("wifi");
                        break;
                    case "4":
                        HandleCyberQuery("social");
                        break;
                    case "5":
                        HandleCyberQuery("purpose");
                        break;
                    case "6":
                        HandleCyberQuery("topics");
                        break;
                    case "7":
                        ShowCyberStats();
                        break;
                    case "8":
                        ShowCyberExitBanner();
                        return;
                    default:
                        ShowTypingEffect("Invalid option! Please select 1-8.", 30, ConsoleColor.Red);
                        break;
                }
            }
        }
        #endregion

        #region Input Validation
        /// <summary>
        /// Gets and validates user's name
        /// </summary>
        /// <returns>Formatted user name</returns>
        private static string GetValidName()
        {
            while (true)
            {
                ShowTypingEffect("🔒 Enter your name (3-20 chars, letters only):", 20, ConsoleColor.White);
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input) || input.Length < 3)
                {
                    ShowTypingEffect("Error: Name must be at least 3 characters!", 30, ConsoleColor.Red);
                    continue;
                }

                if (!Regex.IsMatch(input, @"^[a-zA-Z]+$"))
                {
                    ShowTypingEffect("Error: Only letters allowed!", 30, ConsoleColor.Red);
                    continue;
                }

                return char.ToUpper(input[0]) + input.Substring(1).ToLower();
            }
        }

        /// <summary>
        /// Validates menu choice input
        /// </summary>
        /// <returns>User's choice or default "0" if invalid</returns>
        private static string GetValidatedChoice()
        {
            string input = Console.ReadLine()?.Trim();
            return string.IsNullOrEmpty(input) ? "0" : input;
        }
        #endregion

        #region Cybersecurity Features
        /// <summary>
        /// Handles user queries for specific cybersecurity topics
        /// </summary>
        /// <param name="topic">The selected cybersecurity topic</param>
        private static void HandleCyberQuery(string topic)
        {
            CurrentSession.QueriesAsked++;
            ShowTypingEffect($"=== {topic.ToUpper()} INFORMATION ===", 20, ConsoleColor.Cyan);
            if (Responses.TryGetValue(topic, out List<string> answers))
            {
                foreach (string answer in answers)
                {
                    ShowTypingEffect(answer, 30, ConsoleColor.Cyan);
                }
            }
            ShowTypingEffect("Need more help? Contact info@cybersec.gov.za", 30, ConsoleColor.White);
        }

        /// <summary>
        /// Displays SA cybersecurity statistics
        /// </summary>
        private static void ShowCyberStats()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            ShowTypingEffect(@"
            ┌══════════════ SA CYBER STATS 2024 ═════════════┐
            │ Phishing Attempts: 1.2M reported               │
            │ Ransomware Cases: Significant increase noted   │
            │ Social Engineering: Common via phone scams     │
            │ Wi-Fi Hacks: Prevalent in public networks      │
            └════════════════════════════════════════════════┘
            ", 20, ConsoleColor.Magenta);
            Console.ResetColor();
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Displays text with a typing animation effect
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="speed">Typing speed in milliseconds</param>
        /// <param name="color">Text color</param>
        private static void ShowTypingEffect(string text, int speed, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        /// <summary>
        /// Plays a cybersecurity-themed sound with fallback
        /// </summary>
        private static void PlayCyberSound()
        {
            try
            {
                new SoundPlayer("Audio/cyber_alert.wav").PlaySync();
            }
            catch
            {
                Console.Beep(600, 150);
                Console.Beep(800, 150);
            }
        }
        #endregion
    }
}

