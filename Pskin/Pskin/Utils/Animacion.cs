using System;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace Pskin.Utils
{
    public class Animacion
    {
        /// <summary>
        /// Appear the specified view.
        /// </summary>
        /// <param name="view">View.</param>
        public static async Task Appear(View view)
        {
            for (int i = 0; i <= 20; i++)
            {
                await view.FadeTo((i * 0.05), 20, Easing.Linear);
            }
        }

        /// <summary>
        /// Appear the specified view and milisegundos.
        /// </summary>
        /// <returns>The appear.</returns>
        /// <param name="view">View.</param>
        /// <param name="milisegundos">Milisegundos.</param>
        public static async Task Appear(View view, int milisegundos)
        {
            for (int i = 0; i <= 20; i++)
            {
                await view.FadeTo((i * 0.05), (uint)milisegundos, Easing.Linear);
            }
        }

        /// <summary>
        /// Disappear the specified view.
        /// </summary>
        /// <returns>The disappear.</returns>
        /// <param name="view">View.</param>
        public static async Task Disappear(View view)
        {
            for (int i = 20; i >= 0; i--)
            {
                await view.FadeTo((i * 0.05), 20, Easing.Linear);
            }
        }

        /// <summary>
        /// Disappear the specified view and milisegundos.
        /// </summary>
        /// <returns>The disappear.</returns>
        /// <param name="view">View.</param>
        /// <param name="milisegundos">Milisegundos.</param>
        public static async Task Disappear(View view, int milisegundos)
        {
            for (int i = 20; i >= 0; i--)
            {
                await view.FadeTo((i * 0.05), (uint)milisegundos, Easing.Linear);
            }
        }

        /// <summary>
        /// Seleccionar the specified view.
        /// </summary>
        /// <returns>The seleccionar.</returns>
        /// <param name="view">View.</param>
        public static async Task Seleccionar(View view)
        {
            view.Opacity = 0.8;
            await view.ScaleTo(0.99, 50, Easing.Linear);
            await view.ScaleTo(0.98, 50, Easing.Linear);
            await view.ScaleTo(0.97, 50, Easing.Linear);
        }

        /// <summary>
        /// Deseleccionar the specified view.
        /// </summary>
        /// <returns>The deseleccionar.</returns>
        /// <param name="view">View.</param>
        public static async Task Deseleccionar(View view)
        {
            await view.ScaleTo(0.98, 50, Easing.Linear);
            await view.ScaleTo(0.99, 50, Easing.Linear);
            await view.ScaleTo(1.00, 50, Easing.Linear);
            view.Opacity = 1.0;
        }

    }
}
