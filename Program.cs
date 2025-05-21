using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;

// Namespace for the Cybersecurity Chatbot project
namespace CybersecurityChatbot
{
    // Class to store user session data
    class UserSession
    {
        public string UserName { get; set; }          // User's name
        public int QueriesAsked { get; set; }         // Number of queries asked
        public DateTime SessionStart { get; }         // Session start time
        public string CurrentTopic { get; set; }      // Current conversation topic
        public Dictionary<string, string> UserInfo { get; } // Stores user details (name, favorite topic)

        // Constructor to initialize session
        public UserSession()
        {
            SessionStart = DateTime.Now;
            UserInfo = new Dictionary<string, string>();
        }
    }

    // Main class for the Cybersecurity Chatbot
    class Program
    {
        // Map menu options to topics
        private static readonly Dictionary<string, string> MenuMappings = new()
        {
            { "1", "phishing" },
            { "2", "malware" },
            { "3", "wifi" },
            { "4", "social" },
            { "5", "purpose" },
            { "6", "topics" }
        };

        // Responses for main topics with South African context
        private static readonly Dictionary<string, List<string>> TopicResponses = new()
        {
            { "phishing", new List<string> {
                "Phishing is a major issue in South Africa, with data exposure at 39.19% of cyber incidents (Pieterse, 2021).",
                "Beware of fake SARS eFiling emails – always verify the sender’s authenticity.",
                "Recommendation: Use multi-factor authentication (MFA) on your accounts.",
                "Report suspicious emails to phishing@cybersec.gov.za."
            }},
            { "malware", new List<string> {
                "Malware, including system intrusions at 14.86%, is rising in South Africa (Pieterse, 2021).",
                "Ransomware can lock your files – report to the SAPS Cyber Unit instead of paying.",
                "Best practice: Keep software updated and use antivirus tools like ESET.",
                "Avoid downloading apps from unofficial sources; stick to Google Play or Apple Store."
            }},
            { "wifi", new List<string> {
                "Public Wi-Fi in South Africa, like at Sandton City Mall, is prone to hacks (Pieterse, 2021).",
                "Use a VPN for encryption on public networks – try NordVPN or Surfshark.",
                "Avoid banking logins on public Wi-Fi, especially at places like OR Tambo Airport.",
                "Secure your home Wi-Fi with WPA3 and a strong password."
            }},
            { "social", new List<string> {
                "Social engineering scams, often by hackers (>50%), are common in South Africa (Pieterse, 2021).",
                "Watch out for 'vishing' calls pretending to be SARS or FNB asking for your OTP.",
                "Never share personal details with unsolicited callers – verify their identity.",
                "Contact official numbers like 0800 222 050 for FNB to confirm legitimacy."
            }},
            { "purpose", new List<string> {
                "I’m here to help South African citizens with cybersecurity education.",
                "I provide tips on threats like phishing, malware, and social engineering."
            }},
            { "topics", new List<string> {
                "Ask about phishing, malware, Wi-Fi safety, or social engineering scams.",
                "I can also share South African cyber statistics or safety tips."
            }}
        };

        // Single keyword responses
        private static readonly Dictionary<string, string> SingleKeywordResponses = new()
        {
            { "password", "Use a strong, unique password without personal details." },
            { "scam", "Be cautious of online scams claiming to be from SARS – verify sources." },
            { "privacy", "Adjust privacy settings on accounts, especially on WhatsApp." },
            { "firewall", "A firewall protects your device by blocking unauthorized access – ensure it’s enabled." },
            { "encryption", "Encryption secures your data – use tools like HTTPS for safe browsing." },
            { "vpn", "A VPN encrypts your internet connection, ideal for public Wi-Fi – consider NordVPN." }
        };

        // Multi-keyword responses with random selection
        private static readonly Dictionary<string, List<string>> MultiKeywordResponses = new()
        {
            { "phishing tip", new List<string>
            {
                "Don’t click links in emails claiming to be from Absa or Standard Bank – verify the URL.",
                "Scammers may create urgency, like 'Pay now or lose your SARS refund' – don’t comply.",
                "Enable multi-factor authentication (MFA) for added security.",
                "Forward suspicious emails to phishing@cybersec.gov.za for analysis."
            }},
            { "malware protection", new List<string>
            {
                "Install reputable antivirus software like Kaspersky to detect malware.",
                "Avoid opening email attachments from unknown sources – they may contain malware.",
                "Regularly back up your data to recover from ransomware attacks.",
                "Update your operating system to patch vulnerabilities."
            }},
            { "social engineering tip", new List<string>
            {
                "Be skeptical of unsolicited calls claiming to be from FNB – verify their identity.",
                "Don’t share OTPs or personal details with unknown callers.",
                "Educate yourself on common scams, like fake SARS refund calls.",
                "If in doubt, hang up and call back using official numbers, like 0800 222 050 for FNB."
            }}
        };

        // Sentiment detection keywords
        private static readonly List<string> WorriedKeywords = new() { "worried", "anxious", "scared", "concerned", "nervous" };
        private static readonly List<string> CuriousKeywords = new() { "curious", "interested", "want to know", "wondering" };
        private static readonly List<string> FrustratedKeywords = new() { "frustrated", "confused", "difficult", "stuck" };
        private static readonly List<string> AngryKeywords = new() { "angry", "upset", "annoyed", "irritated" };
        private static readonly List<string> HappyKeywords = new() { "happy", "glad", "pleased", "excited" };

        // Session and utility objects
        private static UserSession CurrentSession;
        private static readonly Random Random = new Random();

        // Program entry point
        static void Main()
        {
            InitializeConsole(); // Set up the console
            PlayWelcomeMessage(); // Play welcome audio or text
            bool startNewSession = true;
            while (startNewSession)
            {
                CurrentSession = new UserSession { UserName = GetUserName() }; // Start a new session
                Console.WriteLine($"Session started for {CurrentSession.UserName} at {DateTime.Now}");
                startNewSession = StartChatSession(); // Run the chat session
            }
        }

        #region Setup and User Interface
        // Configures the console appearance
        private static void InitializeConsole()
        {
            Console.Title = "South African Cybersecurity Shield";
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            DisplayWelcomeBanner();
        }

        // Plays the welcome audio or displays a text fallback
        private static void PlayWelcomeMessage()
        {
            try
            {
                new SoundPlayer("Audio/welcome.wav").PlaySync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audio failed: {ex.Message}");
                Console.WriteLine("Text fallback: Welcome to the Cybersecurity Awareness Bot. I am here to assist you with online safety.");
                Thread.Sleep(2000);
            }
        }

        // Displays the welcome banner
        private static void DisplayWelcomeBanner()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
            ┌════════════════ SOUTH AFRICAN CYBERSECURITY DEPT. ═══════════════┐
            │ Protecting South Africa's Digital Future - Est. 2015           │
            └══════════════════════════════════════════════════════┘
            ");
            Console.ResetColor();
        }

        // Displays the exit banner with session stats
        private static void DisplayExitBanner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
            ┌═══════════════ SECURE SESSION TERMINATION ═════════════┐
            └═══════════════════════════════════════════════════════┘
            ");
            Console.WriteLine($"Session Statistics for {CurrentSession.UserName}:");
            Console.WriteLine($"Duration: {(DateTime.Now - CurrentSession.SessionStart).Minutes} minutes");
            Console.WriteLine($"Queries: {CurrentSession.QueriesAsked}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
            ┌═══════════════ SOUTH AFRICAN CYBERSECURITY RESOURCES ═════════════┐
            │ Emergency Contact: 0800-CYBER-SA (0800-29237-72)                  │
            │ Report Incidents: www.cybersecurityhub.gov.za                     │
            │ Email: info@cybersec.gov.za                                       │
            │ Stay vigilant and protect your digital identity.                  │
            └══════════════════════════════════════════════════════┘
            ");
            Console.ResetColor();
        }

        // Displays the menu with options
        private static void DisplayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
            ┌═════════════════ CYBER MENU ═════════════════┐
            │ 1. Phishing Threats                         │
            │ 2. Malware Protection                       │
            │ 3. Wi-Fi Safety                             │
            │ 4. Social Engineering Scams                 │
            │ 5. What’s Your Purpose?                     │
            │ 6. What Can I Ask About?                    │
            │ 7. South African Cyber Statistics           │
            │ 8. Exit Securely                            │
            │ 9. Start New Session                        │
            └═════════════════════════════════════════════┘
            ");
            Console.ResetColor();
            Console.Write("Enter your choice (1-9): ");
        }
        #endregion

        #region Chat Logic
        // Manages the chat session loop
        private static bool StartChatSession()
        {
            Console.WriteLine($"Welcome, {CurrentSession.UserName}! Your Cybersecurity Shield is active.");

            while (true)
            {
                DisplayMenu();
                string rawInput = Console.ReadLine() ?? "";
                string input = rawInput.Trim().ToLower(); // Normalize input
                string sentiment = DetectUserSentiment(input);
                StoreUserMemory(rawInput);

                if (MenuMappings.ContainsKey(input))
                {
                    string topic = MenuMappings[input];
                    ProcessCyberQuery(topic, sentiment);
                }
                else if (TopicResponses.ContainsKey(input) || SingleKeywordResponses.ContainsKey(input) || MultiKeywordResponses.ContainsKey(input))
                {
                    ProcessCyberQuery(input, sentiment);
                }
                else if (input == "7")
                {
                    DisplayCyberStatistics();
                }
                else if (input == "8" || input == "exit")
                {
                    DisplayExitBanner();
                    Console.WriteLine($"Session ended for {CurrentSession.UserName} at {DateTime.Now}");
                    return false; // Exit the session
                }
                else if (input == "9")
                {
                    Console.WriteLine("Starting a new session...");
                    return true; // Start a new session
                }
                else
                {
                    HandleUnknownInput(rawInput, sentiment);
                }

                // Suggest a related topic based on conversation flow
                SuggestRelatedTopic();
            }
        }

        // Processes user queries based on topic
        private static void ProcessCyberQuery(string topic, string sentiment)
        {
            CurrentSession.QueriesAsked++;
            CurrentSession.CurrentTopic = topic; // Track topic for conversation flow

            Console.WriteLine($"=== {topic.ToUpper()} INFORMATION ===");
            if (TopicResponses.ContainsKey(topic))
            {
                string response = TopicResponses[topic][Random.Next(TopicResponses[topic].Count)];
                Console.WriteLine(response);
            }
            else if (SingleKeywordResponses.ContainsKey(topic))
            {
                Console.WriteLine(SingleKeywordResponses[topic]);
            }
            else if (MultiKeywordResponses.ContainsKey(topic))
            {
                string response = MultiKeywordResponses[topic][Random.Next(MultiKeywordResponses[topic].Count)];
                Console.WriteLine(response);
            }

            RespondBasedOnSentiment(sentiment);
        }

        // Responds based on detected user sentiment
        private static void RespondBasedOnSentiment(string sentiment)
        {
            if (sentiment == "worried")
                Console.WriteLine("I understand your concern. Would you like more security tips?");
            else if (sentiment == "curious")
                Console.WriteLine("Great to see your interest! Want to explore this topic further?");
            else if (sentiment == "frustrated")
                Console.WriteLine("I’m sorry for any confusion. Need further clarification?");
            else if (sentiment == "angry")
                Console.WriteLine("I’m sorry if something upset you. How can I assist you now?");
            else if (sentiment == "happy")
                Console.WriteLine("I’m glad you’re pleased! What else can I help with?");
            else
                Console.WriteLine("For more help, contact info@cybersec.gov.za.");
        }

        // Handles unknown inputs with improved flow
        private static void HandleUnknownInput(string rawInput, string sentiment)
        {
            CurrentSession.QueriesAsked++;
            Console.WriteLine("I can’t process that input. Try keywords like 'phishing', 'malware', 'password', 'firewall', or a menu option (1-9).");

            // Only show sentiment-based follow-up if relevant
            if (sentiment == "worried")
                Console.WriteLine("I’m here to help. What concerns you?");
            else if (sentiment == "curious")
                Console.WriteLine("Let’s find the right info. What do you want to learn?");
            else if (sentiment == "frustrated")
                Console.WriteLine("Sorry for the trouble. Let’s try again – how can I assist?");
            else if (sentiment == "angry")
                Console.WriteLine("I’m sorry if something upset you. How can I assist you now?");
            else if (sentiment == "happy")
                Console.WriteLine("I’m glad you’re pleased! What else can I help with?");
            else
            {
                // Neutral fallback to guide the user without assuming sentiment
                string suggestion = CurrentSession.CurrentTopic != null
                    ? $"You were asking about {CurrentSession.CurrentTopic}. Would you like to continue with that?"
                    : "Try asking about a topic like 'phishing' or 'malware'.";
                Console.WriteLine(suggestion);
            }
        }

        // Suggests a related topic based on conversation history
        private static void SuggestRelatedTopic()
        {
            if (CurrentSession.CurrentTopic == null) return;

            string relatedTopic = CurrentSession.CurrentTopic switch
            {
                "phishing" => "Try 'phishing tip' for more specific advice.",
                "malware" => "Try 'malware protection' for detailed tips.",
                "social" => "Try 'social engineering tip' for practical advice.",
                "wifi" => "Try 'vpn' for more on securing your connection.",
                _ => "Try another topic like 'firewall' or 'encryption'."
            };
            Console.WriteLine($"Suggestion: {relatedTopic}");
        }

        // Displays South African cyber statistics
        private static void DisplayCyberStatistics()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
            ┌══════════════ SOUTH AFRICAN CYBER STATISTICS (2010-2020) ═════════════┐
            │ Total Incidents Analyzed: 74                                          │
            │ Most Common Incident Type: Data Exposure (39.19%)                     │
            │ Most Affected Sector: Public Sector (36%)                             │
            │ Most Prevalent Perpetrators: Hackers (>50%)                           │
            │ Most Common Motivation: Criminal (39.19%)                             │
            │ Source: Pieterse, H. (2021). The Cyber Threat Landscape in South Africa. │
            └════════════════════════════════════════════════════════════════════════┘
            ");
            Console.ResetColor();
        }

        // Detects user sentiment based on input
        private static string DetectUserSentiment(string input)
        {
            string lowerInput = input.ToLower();
            if (WorriedKeywords.Any(k => lowerInput.Contains(k))) return "worried";
            if (CuriousKeywords.Any(k => lowerInput.Contains(k))) return "curious";
            if (FrustratedKeywords.Any(k => lowerInput.Contains(k))) return "frustrated";
            if (AngryKeywords.Any(k => lowerInput.Contains(k))) return "angry";
            if (HappyKeywords.Any(k => lowerInput.Contains(k))) return "happy";
            return "neutral";
        }
        #endregion

        #region Memory and Personalization
        // Stores and recalls user details for personalization
        private static void StoreUserMemory(string input)
        {
            string lowerInput = input.ToLower();
            if (lowerInput.StartsWith("my name is "))
            {
                string name = input.Substring("my name is ".Length).Trim();
                if (!string.IsNullOrEmpty(name) && name.All(char.IsLetter))
                {
                    CurrentSession.UserInfo["name"] = name;
                    // Capitalize the name for display
                    string formattedName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                    Console.WriteLine($"Thank you, {formattedName}. Which cybersecurity topic interests you?");
                }
            }
            else if (lowerInput.Contains("interested in "))
            {
                string topic = input.Substring(lowerInput.IndexOf("interested in ") + "interested in ".Length).Trim();
                if (!string.IsNullOrEmpty(topic))
                {
                    CurrentSession.UserInfo["favoriteTopic"] = topic;
                    Console.WriteLine($"Noted. I’ll remember your interest in {topic.Replace(" ", "-")}.");
                }
            }
            else if (CurrentSession.UserInfo.ContainsKey("name") && CurrentSession.UserInfo.ContainsKey("favoriteTopic") && CurrentSession.CurrentTopic != null)
            {
                if (CurrentSession.CurrentTopic.ToLower() == CurrentSession.UserInfo["favoriteTopic"].ToLower())
                {
                    string name = CurrentSession.UserInfo["name"];
                    string formattedName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                    Console.WriteLine($"As {formattedName}, since you’re interested in {CurrentSession.UserInfo["favoriteTopic"]}, I can provide more details.");
                }
            }
        }
        #endregion

        #region Input Validation
        // Gets and validates the user's name
        private static string GetUserName()
        {
            while (true)
            {
                Console.WriteLine("Please enter your name (3-20 characters, letters only):");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input) || input.Length < 3)
                {
                    Console.WriteLine("Error: Name must be at least 3 characters.");
                    continue;
                }

                if (!input.All(char.IsLetter))
                {
                    Console.WriteLine("Error: Only letters are allowed.");
                    continue;
                }

                return char.ToUpper(input[0]) + input.Substring(1).ToLower();
            }
        }
        #endregion
    }
}