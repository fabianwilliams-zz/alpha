using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using alphatodoService.DataObjects;
using alphatodoService.Models;

namespace alphatodoService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            Database.SetInitializer(new alphatodoInitializer());
        }
    }

    public class alphatodoInitializer : ClearDatabaseSchemaIfModelChanges<alphatodoContext>
    {
        protected override void Seed(alphatodoContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Extended Item One", Complete = false, Owner = "Fabian Williams", Description = "Alpha"},
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Extended Item Two", Complete = false, Owner = "Gabby Williams", Description = "Bravo"},

            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
}

