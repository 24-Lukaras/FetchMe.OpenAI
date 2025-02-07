# FetchMe.OpenAI
Simple library which trivializes conversion of OpenAI ChatClient response to an object.
## Table of Contents
- [Dependencies](#Dependencies)
- [Usage](#Usage)
  - [Specifying response type](#Specifying-response-type)
  - [Creating own fetch request](#Creating-own-fetch-request)
  - [Fetching data with fetch request](#Fetching-data-with-fetch-request)
## Dependencies
This library currently depends on 3 libraries available on NuGet:
- [OpenAI](https://github.com/openai/openai-dotnet) - ChatGPT client
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) - Conversion from chat client response
- [Newtonsoft.Json.Schema](https://github.com/JamesNK/Newtonsoft.Json.Schema) - Schema generation for response format
## Usage
Use of this library is quite simple and can be broken down into 3 steps:
### Specifying response type
First you will need to create a class that will serve as a response of the chat client.

    public class EmailSuggestions
    {
        public List<EmailSuggestion> Suggestions { get; set; } = new List<EmailSuggestion>();
    }

    public class EmailSuggestion
    {        
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
### Creating own fetch request
Afterwards you can create a class implementing Fetch<TResult>. This class will be used as a request wrapper.

    public class SuggestEmail : Fetch<EmailSuggestions>
    {
        public string Purpose { get; set; } = null!;
        public string For { get; set; } = null!;
        public string Style { get; set; } = null!;
        public int Limit { get; set; } = 5;
        public List<string> AdditionalInfo { get; set; } = new List<string>();
    
        public override string JsonSchemaFormatName => "email_suggestions";
    
        public override string Message => $"""
            Provide me with {Limit} suggestions of how to formulate an email as representative {For}.
            The purpose of the email is: {Purpose}.
            Email should be stylized in this manner: {Style}.
            Here is some additional info that should be included in the mail:
            {string.Join(", ", AdditionalInfo)}
            """;
    }    
### Fetching data with fetch request
Now all you need to do is to create instance of the request and call Fetch or FetchAsync method on ChatClient.

    using OpenAI;

    var client = new OpenAIClient(SECRET);
    var chat = client.GetChatClient("gpt-4o-mini");
    
    var emailSuggestionRequest = new SuggestEmail()
    {
        Purpose = "an apology mail to customer regarding unavailability of our service",
        For = "TestCompany inc.",
        Style = "professional",
        Limit = 3,
        AdditionalInfo = new List<string>()
        {
            "problem has been already resolved",
            "the client will get 5% discount for the inconvenience"
        }
    };
    var suggestionResponse = await chat.FetchAsync(emailSuggestionRequest);
