using OpenAI.Chat;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Schema;
using FetchMe.OpenAI.Schema.Generators;
using System;

namespace FetchMe.OpenAI
{
    /// <summary>
    /// Fetch request that contains message for OpenAI chat bot.
    /// </summary>
    /// <typeparam name="TResult">Type of response object.</typeparam>
    public abstract class Fetch<TResult> where TResult : class
    {
        /// <summary>
        /// Name for generated json schema.
        /// </summary>
        public abstract string JsonSchemaFormatName { get; }

        /// <summary>
        /// Formatted message that is sent to OpenAI chat bot.
        /// </summary>
        public abstract string Message { get; }

        private static ChatCompletionOptions? _options;
        /// <summary>
        /// Options provided to <see cref="ChatClient"/> which contains json schema response format. 
        /// </summary>
        internal ChatCompletionOptions Options
        {
            get
            {
                if (_options is null)
                {
                    var schema = GenerateSchema();
                    _options = new ChatCompletionOptions()
                    {
                        ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                            jsonSchemaFormatName: JsonSchemaFormatName,
                            jsonSchema: BinaryData.FromString(schema.ToString()),
                            jsonSchemaIsStrict: true
                        )
                    };
                }
                return _options;
            }
        }

        /// <summary>
        /// Creates a chat message to be sent through <see cref="ChatClient"/>.
        /// </summary>
        /// <returns>Instance of <see cref="UserChatMessage"/>.</returns>
        internal UserChatMessage CreateChatMessage()
        {
            return new UserChatMessage(Message);
        }

        /// <summary>
        /// Generates json schema that is used as a response format.
        /// </summary>
        /// <returns>Json schema of <see cref="TResult"/>.</returns>
        private JSchema GenerateSchema()
        {
            var generator = new JSchemaGenerator();
            generator.GenerationProviders.Add(new StringEnumGenerationProvider());
            generator.GenerationProviders.Add(new DisableAdditionalPropertiesGenerator());
            var schema = generator.Generate(typeof(TResult));
            return schema;
        }
    }
}
