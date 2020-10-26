using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Input.Preview.Injection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DS_AutoMessager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btnAddText_Click(object sender, RoutedEventArgs e)
        {
            if (textInput.Text != "")
            {
                textList.Items.Add(this.textInput.Text);
                textInput.Text = "";
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (textList.SelectedIndex == -1)
            {
                string reply = "please select an item to delete.";
                textError.Text = reply.ToUpper();
                return;
            }
            else 
            {
                int idxSelected = textList.SelectedIndex;
                textList.Items.RemoveAt(idxSelected);
            }

        }

        private void inputTime_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            textError.Text = "";

            //Check if inputTime is not empty
            if (inputTime.Text == "")
            {
                string reply = "please enter a number for time.";
                textError.Text = reply.ToUpper();
                return;
            }

            //Check if ListBox is not empty

            if (textList.Items.Count < 1)
            {
                string reply = "please add text to the list.";
                textError.Text = reply.ToUpper();
                return;
            }


            //Check if timeType is selected
            try
            {
                var comboBoxItem = timeType.Items[timeType.SelectedIndex] as ComboBoxItem;

                if (comboBoxItem != null)
                {
                    string selectedType = comboBoxItem.Content.ToString();

                    if (inputTime.Text != "")
                    {
                        int timeAmount = Int32.Parse(inputTime.Text);
                        int totalTime = 0;

                        if (selectedType == "Seconds")
                        {
                            totalTime = timeAmount * 1000;
                        }
                        else if (selectedType == "Minutes")
                        {
                            totalTime = timeAmount * (60 * 1000);
                        }

                        await Task.Delay(5000);

                        for (int i = 0; i < textList.Items.Count; i++)
                        {
                            string line = textList.Items[i].ToString();

                            startTypingOnKeyboard(line);
                            await Task.Delay(1000);
                            //Debug.WriteLine();
                        }
                    }
                }
            }
            catch {
                string reply = "please select minutes or seconds.";
                textError.Text = reply.ToUpper();
                return;
            }
        }

        static void startTypingOnKeyboard(string textLine) {
            InputInjector inputInjector = InputInjector.TryCreate();

            foreach (var letter in textLine)
            {
                if (letter.ToString() == "!")
                {
                    var shift = new InjectedInputKeyboardInfo();
                    shift.VirtualKey = (ushort)(VirtualKey.Shift);
                    shift.KeyOptions = InjectedInputKeyOptions.None;


                    var one = new InjectedInputKeyboardInfo();
                    one.VirtualKey = (ushort)(VirtualKey.Number1);
                    one.KeyOptions = InjectedInputKeyOptions.None;

                    inputInjector.InjectKeyboardInput(new[] { shift, one });
                }
                else
                {
                    var info = new InjectedInputKeyboardInfo();
                    info.VirtualKey = (ushort)((VirtualKey)Enum.Parse(typeof(VirtualKey), letter.ToString(), true));
                    inputInjector.InjectKeyboardInput(new[] { info });

                }

            }

            var enter = new InjectedInputKeyboardInfo();
            enter.VirtualKey = (ushort)(VirtualKey.Enter);
            enter.KeyOptions = InjectedInputKeyOptions.None;
            inputInjector.InjectKeyboardInput(new[] { enter });
        }
    }
}
