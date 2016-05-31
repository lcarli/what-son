using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace What_Son
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        Random random = new Random();
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                int length = (message.Text ?? string.Empty).Length;

                //Verification for Bad Words
                if (ContainOffensiveWords(myEventHubMessage.ToLower()))
                {
                    return SendCloudToDeviceMessageAsync(OffensiveWordsAnswers[random.Next(OffensiveWordsAnswers.Count())]);
                }
                //Conditionals to verify if you're saying greeting or farewells
                else if (ContainSimpleGrettings(myEventHubMessage.ToLower()))
                {
                    return SendCloudToDeviceMessageAsync(SimpleAnswers[random.Next(SimpleAnswers.Count())]);
                }
                else if (ContainSpecialGrettings(myEventHubMessage.ToLower()))
                {
                    return SendCloudToDeviceMessageAsync(SpecialAnswers[random.Next(SpecialAnswers.Count())]);
                }
                else if (ContainQuestionGrettings(myEventHubMessage.ToLower()))
                {
                    return SendCloudToDeviceMessageAsync(QuestionsAnswers[random.Next(QuestionsAnswers.Count())]);
                }
                else if (ContainSimpleGoodbye(myEventHubMessage.ToLower()))
                {
                    return SendCloudToDeviceMessageAsync(SimpleGoodbyeAnswers[random.Next(SimpleGoodbyeAnswers.Count())]);
                }
                else if (ContainStarWarsGoodbye(myEventHubMessage.ToLower()))
                {
                    return SendCloudToDeviceMessageAsync(StarWarsGoodbyeAnswers[random.Next(StarWarsGoodbyeAnswers.Count())]);
                }
                else if (ContainYouTooGoodbye(myEventHubMessage.ToLower()))
                {
                    return SendCloudToDeviceMessageAsync(youTooGoodbyeAnswers[random.Next(youTooGoodbyeAnswers.Count())]);
                }


                //conditionals to answer something
                //
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=countriestable2;AccountKey=r6uniZc4hXAwyLZLxr1dGzDcXF5mDU4TVriae0F2j5y/+jYoKwkSxTjrU1CbHvHlIGtlzz+/aNVWlR30BBB5HQ==");

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference("countries");

                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<Controllers.CountryEntity>("Brazil", "Brazil");

                // Execute the retrieve operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);



                //default
                return message.CreateReplyMessage(misunderstand[random.Next(misunderstand.Count())]);
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                //
            }
            else if (message.Type == "BotAddedToConversation")
            {
                return message.CreateReplyMessage(SimpleAnswers[random.Next(SimpleAnswers.Count())]);
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
                return message.CreateReplyMessage(SimpleGoodbyeAnswers[random.Next(SimpleGoodbyeAnswers.Count())]);
            }
            else if (message.Type == "UserAddedToConversation")
            {
                return message.CreateReplyMessage(GrettingSimple[random.Next(GrettingSimple.Count())]);
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
                return message.CreateReplyMessage(SimpleGoodbyeAnswers[random.Next(SimpleGoodbyeAnswers.Count())]);
            }
            else if (message.Type == "EndOfConversation")
            {
                return message.CreateReplyMessage(SimpleGoodbyeAnswers[random.Next(SimpleGoodbyeAnswers.Count())]);
            }

            return null;
        }

        #region answersList
        private List<string> SimpleAnswers = new List<string>
        {
            "Hi",
            "Hello",
            "Hello from the death star",
            "Hi, buddy",
            "Hey!",
        };

        private List<string> OffensiveWordsAnswers = new List<string>
        {
            "Hey, Bro! Don't say those words.",
            "Excuse me? Didi you really said that?",
            "Comme on! Keep your mouth shut. Don't say that.",
            "Don't say that, please",
        };

        private List<string> SpecialAnswers = new List<string>
        {
            "About my day? My day was going so nice! I'm breaking my head with those guys to understand about IoT.",
            "My day was going so exaustive. Build IoT stuffs is cool but is exaustive.",
            "It's going very fine, until now.",
            "It's been gorgeous!",
        };

        private List<string> QuestionsAnswers = new List<string>
        {
            "I'm fine, thank you!",
            "I'm doing great!",
            "I'm cool",
            "I'm alright.",
        };

        private List<string> SimpleGoodbyeAnswers = new List<string>
        {
            "Bye bye!",
            "Bye!",
            "It was a great time to talk with you",
            "Have a nice day!",
            "Good luck",
        };

        private List<string> StarWarsGoodbyeAnswers = new List<string>
        {
            "Always!",
            "The force is stronger in the dark side.",
            "The Force is strong in your family, young Skywalker.",
            "While I've been in your side, I will."
        };

        private List<string> youTooGoodbyeAnswers = new List<string>
        {
            "You too.",
            "Try! Because my day was with a lot of IoT...",
            "Thank you",
            "Thank you, Mister. Or should I say Miss?",
        };

        public List<string> misunderstand = new List<string>
        {
            "Oops. I left miss something...",
            "I don't got it. Please, help me.",
            "Something wrong!",
            "I had a misunderstood.",
            "I did not understand.",
        };

        #endregion

        #region MethodsVerify
        private bool ContainSimpleGrettings(string message)
        {
            foreach (var word in GrettingSimple)
            {
                if (message.Contains(word.ToLower())) return true;
            }

            return false;
        }

        private bool ContainOffensiveWords(string message)
        {
            foreach (var word in BadWords)
            {
                if (message.Contains(word.ToLower())) return true;
            }

            return false;
        }

        private bool ContainSpecialGrettings(string message)
        {
            foreach (var word in GrettingSpecial)
            {
                if (message.Contains(word.ToLower())) return true;
            }

            return false;
        }

        private bool ContainQuestionGrettings(string message)
        {
            foreach (var word in GrettingQuestion)
            {
                if (message.Contains(word.ToLower())) return true;
            }

            return false;
        }
        private bool ContainSimpleGoodbye(string message)
        {
            foreach (var word in GoodbyeSimple)
            {
                if (message.Contains(word.ToLower())) return true;
            }

            return false;
        }
        private bool ContainStarWarsGoodbye(string message)
        {
            foreach (var word in GoodbyeStarWars)
            {
                if (message.Contains(word.ToLower())) return true;
            }

            return false;
        }
        private bool ContainYouTooGoodbye(string message)
        {
            foreach (var word in GoodbyeYouToo)
            {
                if (message.Contains(word.ToLower())) return true;
            }

            return false;
        }

        #endregion

        #region Lists
        public List<string> GrettingSimple = new List<string>
        {
            "Hey",
            "Hello",
            "Hey Man",
            "HI",
            "Good to see you",
            "Nice to see you",
            "It’s been a while",
            "Good morning",
            "Good afternoon",
            "Goo everning",
            "Good night",
            "It's nice to meet you",
            "Pleased to meet you",
            "How have you been",
            "How do you do",
            "Yo",
            "Are you OK",
            "You alright",
            "Alright mate",
            "Howdy",
            "Sup",
            "Whazzup",
            "G’day mate",
            "Hiya",
            "How's everything",
            "How are things",
         };

        public List<string> GrettingQuestion = new List<string>
        {
            "How’s it going",
            "How are you doing",
            "What’s up",
            "What’s new",
            "What’s going on",
            "How’s everything",
            "How are things",
            "How’s life",
            "How’s your day",
            "How are you",
        };

        public List<string> GrettingSpecial = new List<string>
        {
            "How’s your day going",
        };

        public List<string> GoodbyeSimple = new List<string>
        {
            "Goodbye",
            "bye",
            "see ya",
            "c ya",
            "c'ya",
            "See you later",
            "C you later",
            "C' you later",
            "C' ya later",
            "C u",
            "Ciao",
            "see you",
            "Talk to you soon",
            "See you next time",
            "See you soon",
        };

        public List<string> GoodbyeStarWars = new List<string>
        {
            "May the force be with you",
            "May the force be with u",
        };

        public List<string> GoodbyeYouToo = new List<string>
        {
            "Take Care",
            "Have a nice day",
        };

        public List<string> BadWords = new List<string>
        {
            "fuck",
            "asshole",
            "shit",
            "damn",
            "doushe",
            "dumb",
            "donga",
            "ass",
            "drat",
            "cunt",
            "motherfuck",
            "mother fuck",
            "motherfucker",
            "mother fucker",
            "fucking",
            "fucker",
            "assfuck",
            "crap",
            "bullshit",
            "butt",
            "idiot",
            "jerk",
            "pussy",
            "bastard",
            "bitch",
            "cock",
            "cum",
            "dick",
            "dildo",
        };

        #endregion
    }
}