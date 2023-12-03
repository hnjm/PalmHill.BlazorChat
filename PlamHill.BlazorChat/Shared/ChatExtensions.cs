﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PalmHill.BlazorChat.Shared.Models;

namespace PalmHill.BlazorChat.Shared
{
    public static class ChatExtensions
    {
        public const string MESSAGE_START = "<|im_start|>";
        public const string MESSAGE_END = "<|im_end|>";
        public const string SYSTEM_MESSAGE_START = MESSAGE_START + "system" + "\n";
        public const string USER_MESSAGE_START = MESSAGE_START + "user" + "\n";
        public const string ASSISTANT_MESSAGE_START = MESSAGE_START + "assistant";

        public static string ToLlamaPromptString(this ChatConversation chatConversation)
        {
            var promptString = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(chatConversation.SystemMessage))
            { 
                promptString.Append(SYSTEM_MESSAGE_START);
                promptString.Append(chatConversation.SystemMessage);
                promptString.Append(MESSAGE_END);
                promptString.Append("\n");
            }

            var messageIndex = 0;

            foreach (var chatMessage in chatConversation.ChatMessages)
            {
                var messageStartString = "";
                switch (chatMessage.Role)
                { 
                    case ChatMessageRole.User:
                        messageStartString = USER_MESSAGE_START;
                        break;
                    case ChatMessageRole.Assistant:
                        messageStartString = ASSISTANT_MESSAGE_START;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Chat conversation is malformed at {nameof(ChatMessage)}[{messageIndex}]: The {nameof(chatMessage.Role)} must be {nameof(ChatMessageRole.Assistant)} or {nameof(ChatMessageRole.User)}");
                }

                promptString.Append(messageStartString);
                promptString.Append(chatMessage.Message);
                promptString.Append(MESSAGE_END);
                promptString.Append("\n");

                messageIndex++;
            }

            promptString.Append(ASSISTANT_MESSAGE_START);

            var prompt = promptString.ToString();
            return prompt;
        }


    }
}
