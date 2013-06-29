using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using POEApi.Infrastructure;

namespace Procurement.Controls
{
    internal class ForumExportTemplateReader
    {
        private static string template = string.Empty;
        private const string templateFileName = "ForumExportTemplate.txt";

        internal static string GetTemplate()
        {
            try
            {
                if (template != string.Empty)
                    return template;

                template = getTemplateFromDisk();

                if (template != string.Empty)
                    return template;

                template = getDefaultTemplate();
                saveTemplate(template);

                return template;
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
                var message = "Failed to load ForumExportTemplate!";
                Logger.Log(message);
                throw new Exception(message);
            }
        }

        internal static void SaveTemplate(string Template)
        {
            template = Template;
            saveTemplate(template);
        }

        private static void saveTemplate(string defaultTemplate)
        {
            try
            {
                File.WriteAllText(templateFileName, defaultTemplate);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("Failed saving ForumExportTemplate to disk!");
            }
        }

        private static string getDefaultTemplate()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Procurement.ForumExportTemplate.txt"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static string getTemplateFromDisk()
        {
            if (!File.Exists(templateFileName))
                return string.Empty;

            return System.IO.File.ReadAllText(templateFileName);
        }
    }
}
