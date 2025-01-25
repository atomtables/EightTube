using EightTube.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace EightTube.API
{
    [DataContract]
    public abstract class Invidious
    {

        public static string FormatNumber(long viewCount) // didnt feel like changing the variable
        {
            if (viewCount < 1000)
            {
                return "" + viewCount;
            }
            else if (viewCount < 100000)
            {
                return $"{viewCount / 1000:F1}K";
            }
            else if (viewCount < 1000000)
            {
                return $"{viewCount / 1000:F0}K";
            }
            else if (viewCount < 100000000)
            {
                return $"{viewCount / 1000000:F1}M";
            }
            else if (viewCount < 1000000000)
            {
                return $"{viewCount / 1000000:F0}M";
            }
            else if (viewCount < 100000000000)
            {
                return $"{viewCount / 1000000000:F1}B";
            }
            return "" + viewCount;
        }
        public static DateTime ConvertEpochToDateTime(long epoch)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToUniversalTime();
        }
        public static string FormatDate(DateTime publishedDate) // it's almost like im really lazy !!!!
        {
            // There is no way for C# to do this for us, so enjoy the pain...
            TimeSpan difference = DateTime.Now.ToUniversalTime().Subtract(publishedDate.ToUniversalTime());

            if (difference.Minutes == 0 && difference.Hours == 0 && difference.Days == 0)
            {
                return $"{difference.Seconds} second{(difference.Seconds == 1 ? "" : "s")} ago";
            }
            else if (difference.Hours == 0 && difference.Days == 0)
            {
                return $"{difference.Minutes} minute{(difference.Minutes == 1 ? "" : "s")} ago";
            }
            else if (difference.Days == 0)
            {
                return $"{difference.Hours} hour{(difference.Hours == 1 ? "" : "s")} ago";
            }
            // hard part: no one cares about leap years (sorry 2/29 kids)
            else if (difference.Days / 30 == 0) // median months
            {
                return $"{difference.Days} day{(difference.Days == 1 ? "" : "s")} ago";
            }
            else if (difference.Days / 365 == 0) // length of a regular year
            {
                return $"{difference.Days / 30} month{(difference.Days / 30 == 1 ? "" : "s")} ago";
            }
            else // some dinosaur video like a guy at the zoo
            {
                return $"{difference.Days / 365} year{(difference.Days / 365 == 1 ? "" : "s")} ago";
            }
        }

        protected static async Task<string> load(string load, string api)
        {
            await SystemProgressIndicator.show(load);

            HttpResponseMessage response = await Preferences.Client.GetAsync(api);

            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());

                if ((await response.Content.ReadAsStringAsync()).Contains("Rotating Proxies"))
                {
                    await SystemProgressIndicator.hide();
                    throw new DataLoadException((int)response.StatusCode, "This instance is currently rotating proxies. Try again in a few seconds.");
                }

                if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    await SystemProgressIndicator.hide();
                    // hope the system sent some data
                    string error = ((Error)new DataContractJsonSerializer(typeof(Error)).ReadObject(stream))?.error;
                    throw new DataLoadException((int)response.StatusCode, error ?? "An unknown error occured.");
                }
            }
        }

        protected static async Task<T> load<T>(string api)
        {
            HttpResponseMessage response = await Preferences.Client.GetAsync(api);

            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());

                if ((await response.Content.ReadAsStringAsync()).Contains("Rotating Proxies"))
                {
                    await SystemProgressIndicator.hide();
                    throw new DataLoadException((int)response.StatusCode, "This instance is currently rotating proxies. Try again in a few seconds.");
                }

                if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299)
                {
                    await SystemProgressIndicator.hide();
                    return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
                }
                else
                {
                    await SystemProgressIndicator.hide();
                    // hope the system sent some data
                    string error = ((Error)new DataContractJsonSerializer(typeof(Error)).ReadObject(stream))?.error;
                    throw new DataLoadException((int)response.StatusCode, error ?? "An unknown error occured.");
                }
            }
        }

        public static async Task<T> load<T>(string load, string api)
        {
            await SystemProgressIndicator.show(load);

            return await load<T>(api);
        }

        public static async Task<T> load<T>(string load, string api, string arg)
        {
            await SystemProgressIndicator.show(load);

            return await load<T>(api + $"/{arg}");
        }

        public static async Task<T> load<T>(string load, string api, string arg, Tuple<String, String> query)
        {
            await SystemProgressIndicator.show(load);
            return await load<T>(api + $"/{arg}?{query.Item1}={query.Item2}");
        }

        public static async Task catchErrors(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (DataLoadException exception)
            {
                var messageDialog = new MessageDialog($"An error occured while attempting to load data from the server. The server returned: \n\n{exception.Message}\n\nTry again later, check your internet, or switch instances.");
                messageDialog.Commands.Add(new UICommand("Close"));
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"There was some sort of error: {exception}");
                await SystemProgressIndicator.hide();
                var messageDialog = new MessageDialog($"An unknown error of type {exception.GetType()} occured. Try again later, check your internet, or switch instances.");
                messageDialog.Commands.Add(new UICommand("Close"));
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
            }
        }

        public static async Task catchErrors(Func<Task> func, Action<IUICommand> command)
        {
            try
            {
                await func();
            }
            catch (DataLoadException exception)
            {
                var messageDialog = new MessageDialog($"An error occured while attempting to load data from the server. The server returned: \n\n{exception.Message}\n\nTry again later, check your internet, or switch instances.");
                messageDialog.Commands.Add(new UICommand(
                    "Try again",
                    new UICommandInvokedHandler(command)));
                messageDialog.Commands.Add(new UICommand("Close"));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 1;
                await messageDialog.ShowAsync();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"There was some sort of error: {exception}");
                await SystemProgressIndicator.hide();
                var messageDialog = new MessageDialog($"An unknown error of type {exception.GetType()} occured. Try again later, check your internet, or switch instances.");
                messageDialog.Commands.Add(new UICommand(
                    "Try again",
                    new UICommandInvokedHandler(command)));
                messageDialog.Commands.Add(new UICommand("Close"));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 1;
                await messageDialog.ShowAsync();
            }
        }

        public static async Task catchErrors(Func<Task> func, Action<IUICommand> command, Action<IUICommand> onClose)
        {
            try
            {
                await func();
            }
            catch (DataLoadException exception)
            {
                var messageDialog = new MessageDialog($"An error occured while attempting to load data from the server. The server returned: \n\n{exception.Message}\n\nTry again later, check your internet, or switch instances.");
                messageDialog.Commands.Add(new UICommand(
                    "Try again",
                    new UICommandInvokedHandler(command)));
                messageDialog.Commands.Add(new UICommand("Close", new UICommandInvokedHandler(onClose)));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 1;
                await messageDialog.ShowAsync();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"There was some sort of error: {exception}");
                await SystemProgressIndicator.hide();
                var messageDialog = new MessageDialog($"An unknown error of type {exception.GetType()} occured. Try again later, check your internet, or switch instances.");
                messageDialog.Commands.Add(new UICommand(
                    "Try again",
                    new UICommandInvokedHandler(command)));
                messageDialog.Commands.Add(new UICommand("Close", new UICommandInvokedHandler(onClose)));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 1;
                await messageDialog.ShowAsync();
            }
        }
    }
}
