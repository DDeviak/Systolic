using CommunityToolkit.Mvvm.ComponentModel;

namespace Systolic.UI.Models;

public partial class Register : ObservableObject
{
	[ObservableProperty] private string _name = null!;
	
	[ObservableProperty] private double _value;
	
	[ObservableProperty] private Expression _operation = null!;
}