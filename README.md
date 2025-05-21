Cybersecurity Awareness Chatbot (PROG6221 POE Part 2)
A console-based chatbot for PROG6221 POE Part 2, designed to educate South African citizens on cybersecurity topics like phishing, malware, Wi-Fi safety, and social engineering. This project builds on Part 1 with features like keyword recognition, sentiment detection, and better user interaction.
How to Run

Clone the repository:git clone https://github.com/PROG6221-GR04-RC-PTA-2025/prog6221-part-1-submission-hub-ST18345327.git


Navigate to the project folder:cd PROG6221-POE-PART2/CybersecurityChatbot


Ensure .NET 6.0 or later is installed.
Place welcome.wav in the Audio folder (CybersecurityChatbot/Audio/welcome.wav).
Restore dependencies:dotnet restore


Build and run:dotnet run



Features

Plays a welcome audio: "Hello! Welcome to the South African Cybersecurity Awareness Chatbot. I’m here to assist you in staying safe online."
Interactive menu with options 1-9, including "Start New Session."
Validates user name and menu choices.
Provides responses for topics like phishing and malware, using South African examples (e.g., SARS, FNB).
Recognizes keywords (e.g., "password," "firewall," "phishing tip").
Gives random responses for variety.
Tracks conversation and suggests related topics.
Remembers user name and favorite topic.
Detects sentiments (e.g., worried, curious, angry) and adjusts responses.

Requirements

.NET 6.0 or later.
welcome.wav file in the Audio folder.
System.Media reference in CybersecurityChatbot.csproj:<ItemGroup>
  <Reference Include="System.Media" />
</ItemGroup>



Usage

Choose options 1-9 from the menu (e.g., 1 for Phishing Threats).
Use keywords like "phishing" or "malware protection."
Set your name: "my name is [name]" (e.g., "my name is John").
Set a favorite topic: "interested in [topic]" (e.g., "interested in phishing").
Example:
Input: "my name is John"
Output: "Thank you, John. Which cybersecurity topic interests you?"
Input: "phishing tip"
Output: "Enable multi-factor authentication (MFA) for added security."



