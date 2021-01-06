using System;

namespace FreeScoreBoard.Core.Objekte
{
	public class ClsTableFilter
	{
		string myField = "";
		string myValue = "";
		string myTable = "";
		int myIntValue = 0;

		public ClsTableFilter() { }

		public ClsTableFilter(string Table, string Field, string Value)
		{
			this.myTable = Table;
			this.myField = Field;
			this.myValue = Value;
		}

		public ClsTableFilter(string Table, string Field, int IntValue)
		{
			this.myTable = Table;
			this.myField = Field;
			this.myIntValue = IntValue;
		}

		public string Table
		{
			get
			{
				return this.myTable;
			}

			set
			{
				this.myTable = value;
			}
		}

		public string Field
		{
			get
			{
				return this.myField;
			}

			set
			{
				this.myField = value;
			}
		}

		public string Value
		{
			get
			{
				return this.myValue;
			}

			set
			{
				this.myValue = value;
			}
		}

		public int IntValue
		{
			get
			{
				return this.myIntValue;
			}

			set
			{
				this.myIntValue = value;
			}
		}
	}
}
