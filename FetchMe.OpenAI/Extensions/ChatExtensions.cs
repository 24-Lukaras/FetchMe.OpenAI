using FetchMe.OpenAI;
using Newtonsoft.Json;
using OpenAI.Chat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI
{
    /// <summary>
    /// Extensions for fetching results with <see cref="ChatClient"/>.
    /// </summary>
    public static class ChatExtensions
    {
        /// <summary>
        /// Asynchronous method that fetches an AI response and converts it into object.
        /// </summary>
        /// <typeparam name="T">Response type of the fetch request.</typeparam>
        /// <param name="chat">OpenAI chat client.</param>
        /// <param name="fetch">Implementation of <see cref="FetchMe.OpenAI.Fetch{TResult}"/> request.</param>
        /// <returns>OpenAI response converted into return type of corresponding fetch request.</returns>
        public static async Task<T?> FetchAsync<T>(this ChatClient chat, Fetch<T> fetch) where T : class
        {
            List<ChatMessage> messages = new List<ChatMessage>() { fetch.CreateChatMessage() };
            var result = await chat.CompleteChatAsync(messages, fetch.Options);
            return JsonConvert.DeserializeObject<T>(result.Value.Content[0].Text);
        }

        /// <summary>
        /// Method that fetches an AI response and converts it into object.
        /// </summary>
        /// <typeparam name="T">Response type of the fetch request.</typeparam>
        /// <param name="chat">OpenAI chat client.</param>
        /// <param name="fetch">Implementation of <see cref="FetchMe.OpenAI.Fetch{TResult}"/> request.</param>
        /// <returns>OpenAI response converted into return type of corresponding fetch request.</returns>
        public static T? Fetch<T>(this ChatClient chat, Fetch<T> fetch) where T : class
        {
            List<ChatMessage> messages = new List<ChatMessage>() { fetch.CreateChatMessage() };
            var result = chat.CompleteChat(messages, fetch.Options);
            return JsonConvert.DeserializeObject<T>(result.Value.Content[0].Text);
        }
    }

}

