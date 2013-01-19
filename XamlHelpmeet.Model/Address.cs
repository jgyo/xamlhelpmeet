namespace XamlHelpmeet.Model
{
	/// <summary>
	/// 	Address.
	/// </summary>
	public class Address
	{
		/// <summary>
		/// 	Model class for an Address.
		/// </summary>
		/// <param name="street">
		/// 	A street name.
		/// </param>
		/// <param name="streetTwo">
		/// 	A second street name.
		/// </param>
		/// <param name="city">
		/// 	The name of a city.
		/// </param>
		/// <param name="state">
		/// 	The state where the city exists.
		/// </param>
		/// <param name="zip">
		/// 	The address' zip code.
		/// </param>
		public Address(string street, string streetTwo, string city, string state, string zip)
		{
			Zip = zip;
			State = state;
			City = city;
			StreetTwo = streetTwo;
			Street = street;
		}

		/// <summary>
		/// 	Gets or sets the city's name.
		/// </summary>
		/// <value>
		/// 	The city.
		/// </value>
		public string City { get; set; }

		/// <summary>
		/// 	Gets or sets the state.
		/// </summary>
		/// <value>
		/// 	The state.
		/// </value>
		public string State { get; set; }

		/// <summary>
		/// 	Gets or sets the street name.
		/// </summary>
		/// <value>
		/// 	A street.
		/// </value>
		public string Street { get; set; }

		/// <summary>
		/// 	Gets or sets a second street name.
		/// </summary>
		/// <value>
		/// 	The second street name.
		/// </value>
		public string StreetTwo { get; set; }

		/// <summary>
		/// 	Gets or sets the zip code.
		/// </summary>
		/// <value>
		/// 	The zip code.
		/// </value>
		public string Zip { get; set; }
	}
}