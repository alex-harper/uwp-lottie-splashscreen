using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Lottie;
using Windows.Storage.Streams;
using Windows.Storage;
using System.IO;
using Windows.UI;

namespace UWP_Lottie_SplashScreen.Helpers
{
    class AnimatedSplashScreenHelpers
    {
        /// <summary>
        /// Runs a given lottie animation full screen
        /// </summary>
        /// <param name="animationUri">eg: ms-appx:///Assets/splash.json</param>
        /// <returns></returns>
        public static async Task PlayAsync(Uri animationUri)
        {
            //The window needs to be active for the animation to play
            Window.Current.Activate();

            var originalWindowContent = Window.Current.Content;

            //Create the element tree manually
            var splashBorder = new Border
            {
                Padding = new Thickness(40),
                Background = new SolidColorBrush(Colors.White)
            };

            var source = new LottieVisualSource();
            var player = new AnimatedVisualPlayer
            {
                Stretch = Stretch.Uniform,
                AutoPlay = false,
                Source = source
            };

            var stream = await GetAssetInputStream(animationUri);
            await source.SetSourceAsync(stream);

            splashBorder.Child = player;

            Window.Current.Content = splashBorder;

            await player.PlayAsync(0, 1, false);

            // Reset window content after the splashscreen animation has completed.
            Window.Current.Content = originalWindowContent;
        }
        
        private static async Task<IInputStream> GetAssetInputStream(Uri assetPath)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(assetPath);
            var stream = await file.OpenStreamForReadAsync();

            return stream.AsInputStream();
        }
    }
}
