using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq.Expressions;

namespace CustomSlider
{
    public delegate T Transform<T> (T input );

    public class CustomSliderControl : Slider
    {

        private void OnExpressionSet()
        {
            LazySliderFunc = new Lazy<Transform<double>>(() =>
                        {
                            if (CustomSliderValueExpression != null)
                            {
                                CustomerSliderExpressionBody = CustomSliderValueExpression.Body.ToString();
                                return CustomSliderValueExpression.Compile();
                            }
                            else
                                CustomerSliderExpressionBody = "No Expression Specified";
                                return (n) => n;
                        });

        }


        #region CalculatedValue (Dependency Property)
        /// <summary>
        /// The <see cref="CalculatedValue" /> dependency property's name.
        /// </summary>
        public const string CalculatedValuePropertyName = "CalculatedValue";

        /// <summary>
        /// Gets or sets the value of the <see cref="CalculatedValue" />
        /// property. This is a dependency property.
        /// </summary>
        public double CalculatedValue
        {
            get
            {
                return (double)GetValue(CalculatedValueProperty);
            }
            private set
            {
                SetValue(CalculatedValueProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="CalculatedValue" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CalculatedValueProperty = DependencyProperty.Register(
            CalculatedValuePropertyName,
            typeof(double),
            typeof(CustomSliderControl),
            new PropertyMetadata(0D));
        #endregion

        #region CustomerSliderExpressionBody (Dependency Property)
        /// <summary>
        /// The <see cref="CustomerSliderExpressionBody" /> dependency property's name.
        /// </summary>
        public const string CustomerSliderExpressionBodyPropertyName = "CustomerSliderExpressionBody";

        /// <summary>
        /// Gets or sets the value of the <see cref="CustomerSliderExpressionBody" />
        /// property. This is a dependency property.
        /// </summary>
        public string CustomerSliderExpressionBody
        {
            get
            {
                return GetValue(CustomerSliderExpressionBodyProperty).ToString();
            }
            private set
            {
                SetValue(CustomerSliderExpressionBodyProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="CustomerSliderExpressionBody" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CustomerSliderExpressionBodyProperty = DependencyProperty.Register(
            CustomerSliderExpressionBodyPropertyName,
            typeof(string),
            typeof(CustomSliderControl),
            new PropertyMetadata(""));
        #endregion

        private Expression<Transform<double>> _CustomSliderValueExpression;
        /// <summary>
        /// Gets or sets the custom slider value delegate.
        /// </summary>
        /// <value>The custom slider value delegate.</value>
        public Expression<Transform<double>> CustomSliderValueExpression
        {
            get
            {
                return _CustomSliderValueExpression;
            }
            set
            {
                _CustomSliderValueExpression = value;
                OnExpressionSet();
            }
        }

        private Lazy<Transform<double>> LazySliderFunc;

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            CalculatedValue = LazySliderFunc.Value(newValue);
            base.OnValueChanged(oldValue, newValue);
        }


    }
}
