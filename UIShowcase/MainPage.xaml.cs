using DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UIShowcase.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UIShowcase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public MainPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>();

            MyListView.ItemsSource = Items;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            List<string> requests = await DB.Instance.GetAllRequests();

            if (requests == null)
            {
                return;
            }

            Items.Clear();
            foreach (string currentRequest in requests)
            {
                Items.Add(currentRequest);
            }

            if (string.IsNullOrEmpty(APPSettings.FavString))
            {
                await Navigation.PushAsync(new FavoritePicker());
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            // Getting
            Entry textArea = (sender as Entry);
            if (string.IsNullOrWhiteSpace(textArea.Text) || string.IsNullOrEmpty(textArea.Text))
            {
                return;
            }
            string newRequest = textArea.Text.Trim().ToLower();

            // Adding
            DB.Instance.AddNewRequest(newRequest);
            Items.Add(newRequest);

            // Decoding
            // The format must be Greeting | ... <pageType> | [<viewType> <property>] separated by ,
            // EX:
            // Hi | I want a stack | label fav, label GG, Image, idk, something, label, datepicker
            // Hi | I want a scroll | label fav, label GG, webview, boxview, boxview fav, Image, idk, something, label, datepicker, stepper, slider
            string[] sections = newRequest.Split('|');
            string greeting = sections[0].Trim(); // I don't use this..
            string pageSection = sections[1].Trim();
            string viewSection = sections[2].Trim();

            string pageContainerS = pageSection.Split(' ').Last();
            StackLayout pageContainer = new StackLayout
            {
                Spacing = 10
            };

            foreach (string token in viewSection.Split(','))
            {
                string[] toParse = token.Trim().Split();
                View toADD = null;

                string viewType = toParse.First();
                string viewProperty = toParse.Last();

                switch (viewType)
                {
                    case "label":
                        {
                            toADD = new Label
                            {
                                Text = viewType == viewProperty
                                ? string.Empty
                                : (viewProperty == "fav" ? APPSettings.FavString : viewProperty)
                            };

                            break;
                        }
                    case "entry":
                        {
                            toADD = new Entry
                            {
                                Placeholder = viewType == viewProperty
                                ? string.Empty
                                : (viewProperty == "fav" ? APPSettings.FavString : viewProperty)
                            };

                            break;
                        }
                    case "image":
                        {
                            toADD = new Image
                            {
                                Source = ImageSource.FromUri(new Uri(viewType == viewProperty
                                ? @"https://cms-assets.tutsplus.com/uploads/users/244/posts/21644/preview_image/xamarin-preview-image@2x.jpg"
                                : viewProperty))
                            };

                            break;
                        }
                    case "boxview":
                        {
                            toADD = new BoxView
                            {
                                HeightRequest = 60,
                                WidthRequest = 60,
                                HorizontalOptions = LayoutOptions.Center,
                                Color = viewType == viewProperty
                                ? Color.Red
                                : Color.FromHex((viewProperty == "fav" ? APPSettings.FavColor : viewProperty)
                                )
                            };

                            break;
                        }
                    case "webview":
                        {
                            toADD = new WebView
                            {
                                Source =  @"https://academy.microsoft.pub.ro/"
                            };

                            break;
                        }
                    // We skip OpenGL View, it is bad
                    // And Map, I am too lazy for that
                    case "button":
                        {
                            toADD = new Button
                            {
                                Text = viewType == viewProperty
                                ? string.Empty
                                : (viewProperty == "fav" ? APPSettings.FavString : viewProperty)
                            };

                            break;
                        }
                    case "imagebutton":
                        {
                            toADD = new ImageButton
                            {
                                Source = ImageSource.FromUri(new Uri(viewType == viewProperty
                                ? @"https://cms-assets.tutsplus.com/uploads/users/244/posts/21644/preview_image/xamarin-preview-image@2x.jpg"
                                : viewProperty))
                            };

                            break;
                        }
                    case "searchbar":
                        {
                            toADD = new SearchBar
                            {
                                Placeholder = viewType == viewProperty
                                ? string.Empty
                                : (viewProperty == "fav" ? APPSettings.FavString : viewProperty)
                            };

                            break;
                        }
                    case "slider":
                        {
                            toADD = new Slider
                            {
                                Minimum = 0,
                                Maximum = 2147483647, // Hex Max Value
                                Value = double.Parse(viewType == viewProperty
                                ? "5"
                                : (viewProperty == "fav" 
                                    ? Convert.ToDouble(Convert.ToByte(APPSettings.FavColor, 16)).ToString() 
                                        : viewProperty))
                            };

                            break;
                        }
                    case "stepper":
                        {
                            toADD = new Stepper
                            {
                                Minimum = 0,
                                Maximum = 2147483647, // Hex Max Value
                                Value = double.Parse(viewType == viewProperty
                                ? "5"
                                : (viewProperty == "fav" 
                                    ? Convert.ToDouble(Convert.ToByte(APPSettings.FavColor, 16)).ToString()
                                    : viewProperty))
                            };

                            break;
                        }
                    case "switch":
                        {
                            toADD = new Switch
                            {
                                IsToggled = bool.Parse(viewType == viewProperty
                                ? ((new Random().Next() & 1) == 0).ToString() // Random bool generator
                                : (viewProperty == "fav" ? APPSettings.FavBool.ToString() : viewProperty))
                            };

                            break;
                        }
                    case "datepicker":
                        {
                            toADD = new DatePicker
                            {
                                Date = DateTime.Parse(viewType == viewProperty
                                ? DateTime.Now.ToString()
                                : viewProperty)
                            };

                            break;
                        }
                    case "timepicker":
                        {
                            toADD = new TimePicker
                            {
                                Time = DateTime.Parse(viewType == viewProperty
                                ? DateTime.Now.ToString()
                                : viewProperty).TimeOfDay
                            };

                            break;
                        }
                }

                if (toADD != null)
                {
                    pageContainer.Children.Add(toADD);
                }
            }

            View pageContent = pageContainer;

            if (pageContainerS == "scroll")
            {
                pageContent = new ScrollView
                {
                    Content = pageContainer
                };
            }

            if (pageContainerS == "grid")
            {
                Grid pageGrid = new Grid {RowSpacing = 10 };
                int childIndex = 0;

                foreach (View child in pageContainer.Children)
                {
                    pageGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    pageGrid.Children.Add(child, 0, childIndex++);
                }

                pageContent = pageGrid;
            }

            await Navigation.PushAsync(new ContentPage
            {
                Content = pageContent
            });


            textArea.Text = string.Empty;
        }
    }
}
