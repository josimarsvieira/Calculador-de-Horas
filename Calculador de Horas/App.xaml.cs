using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace CalculadorDeHoras
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Sobrescrita do metodo OnStartup configurando a cultura local
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
            XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            AppCenter.Start("49d5212c-bc77-4a21-9f8b-1064e06d4190",
                   typeof(Analytics), typeof(Crashes));
        }
    }
}