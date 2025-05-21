Cybersecurity Awareness Chatbot (PROG6221 POE Part 2)
A console-based chatbot developed for PROG6221 POE Part 2 to educate South African citizens on cybersecurity topics such as phishing, malware, Wi-Fi safety, and social engineering. This project builds on Part 1 by adding advanced features like keyword recognition, sentiment detection, user memory, random responses, and an improved conversation flow.
How to Run

Clone the repository from GitHub Classroom:git clone https://github.com/PROG6221-GR04-RC-PTA-2025/prog6221-part-1-submission-hub-ST18345327.git


Navigate to the project folder:cd prog6221-part-1-submission-hub-ST18345327


Ensure you have .NET installed (e.g., .NET 6.0 or later).
Place the welcome.wav file in the Audio folder (e.g., cybersecurityChatbot/Audio/welcome.wav).
Restore dependencies:dotnet restore


Build and run the application:dotnet run



Features

Plays a voice greeting on startup: "Welcome to the Cybersecurity Awareness Bot. I am here to assist you with online safety." (with text fallback if audio fails).
Interactive menu with cybersecurity topics and a "Start New Session" option.
Input validation for user name (3-20 characters, letters only) and menu choices.
Predefined responses for topics like phishing, malware, Wi-Fi safety, and social engineering, with South African context (e.g., SARS, FNB).
Keyword Recognition: Recognizes keywords like "password," "scam," "privacy," "firewall," "encryption," "vpn," "phishing tip," "malware protection," and "social engineering tip."
Random Responses: Provides varied responses for topics and multi-keyword queries (e.g., "phishing tip").
Conversation Flow: Maintains context by tracking the current topic and suggesting related topics (e.g., after "phishing," suggests "phishing tip").
Memory and Personalization: Remembers user name and favorite topic for personalized responses.
Sentiment Detection: Detects emotions (worried, curious, frustrated, angry, happy) and adjusts responses accordingly.
String Manipulation: Processes user input with methods like Trim(), ToLower(), and Substring() for better interaction.
Improved conversation flow to address issues (e.g., avoids assuming sentiment after unknown inputs, as shown in the provided screenshot).

Requirements

.NET 6.0 or later.
A WAV file named welcome.wav in the Audio directory for the voice greeting.
System.Media reference in the .csproj file for audio playback:<ItemGroup>
  <Reference Include="System.Media" />
</ItemGroup>


Usage

Menu Options: Choose from options 1-9 (e.g., 1 for Phishing Threats, 9 to Start New Session, 8 to Exit).
Keywords: Use keywords like "phishing," "firewall," "malware protection," or "social engineering tip" to get information.
Set Name: Type "my name is [name]" (e.g., "my name is Sarah") to personalize responses.
Set Favorite Topic: Type "interested in [topic]" (e.g., "interested in phishing") to set a favorite topic for tailored responses.
Example Interaction:
Input: "my name is Alice"
Output: "Thank you, Alice. Which cybersecurity topic interests you?"
Input: "interested in phishing"
Output: "Noted. I’ll remember your interest in phishing."
Input: "phishing tip"
Output: (Random tip) "Enable multi-factor authentication (MFA) for added security."
Input: "hello"
Output: "I can’t process that input. You were asking about phishing. Would you like to continue with that?"