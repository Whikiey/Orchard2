﻿using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orchard.Recipes.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Orchard.Recipes.Services
{
    public class JsonRecipeParser : IRecipeParser
    {
        public RecipeDescriptor ParseRecipe(Stream recipeStream)
        {
            var serializer = new JsonSerializer();

            using (StreamReader streamReader = new StreamReader(recipeStream))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    return serializer.Deserialize<RecipeDescriptor>(reader);
                }
            }
        }

        public void ProcessRecipe(
            Stream recipeStream, 
            Action<RecipeDescriptor, RecipeStepDescriptor> action)
        {
            Stream recipeStreamCopy = new MemoryStream();
            recipeStream.CopyTo(recipeStreamCopy);

            var descriptor = ParseRecipe(recipeStreamCopy);

            var serializer = new JsonSerializer();

            using (StreamReader streamReader = new StreamReader(recipeStream))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    // Go to Steps, then iterate.
                    while (reader.Read()) {
                        if (reader.Path == "steps" && reader.TokenType == JsonToken.StartArray)
                        {
                            int stepId = 0;
                            while (reader.Read() && reader.Depth > 1)
                            {
                                if (reader.Depth == 2)
                                {
                                    var child = JToken.Load(reader);
                                    action(descriptor, new RecipeStepDescriptor
                                    {
                                        Id = (stepId++).ToString(CultureInfo.InvariantCulture),
                                        RecipeName = descriptor.Name,
                                        Name = child.Value<string>("name"),
                                        Step = child
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}