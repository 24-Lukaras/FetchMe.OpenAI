using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Schema;

namespace FetchMe.OpenAI.Schema.Generators
{

    /// <summary>
    /// Ensures all objects in json schema contain "additionalProperties": false.
    /// This is required when using strict json schema in response format from OpenAI.
    /// </summary>
    internal class DisableAdditionalPropertiesGenerator : JSchemaGenerationProvider
    {

        /// <inheritdoc/>
        public override JSchema? GetSchema(JSchemaTypeGenerationContext context)
        {
            JSchema schema = context.Generator.Generate(context.ObjectType);
            if (schema.Type == JSchemaType.Object)
            {
                schema.AllowAdditionalProperties = false;
                schema.AllowAdditionalPropertiesSpecified = true;
            }
            return schema;
        }
    }
}