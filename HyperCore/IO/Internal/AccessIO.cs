using ADOX;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace HyperCore.IO
{
	internal class AccessIO
	{
		private static OleDbConnection conn;

		/// <summary>
		/// Initializes a new instance of the AccessIO class.
		/// </summary>
		public AccessIO()
		{
			if (!File.Exists(new OleDbConnectionStringBuilder(Resource.ConnectionCommandAccess).DataSource))
			{
				Create();
			}

		}

		private void Open()
		{
			if (conn == null)
				conn = new OleDbConnection(Resource.ConnectionCommandAccess);
			if (conn.State == ConnectionState.Closed)
				conn.Open();
		}

		private void Create()
		{

			CatalogClass cat = new CatalogClass();
			cat.Create(Resource.ConnectionCommandAccess);
			cat = null;
		}

		private void Close()
		{
			if (conn != null)
				conn.Close();
			conn = null;
		}

		private OleDbCommand CreateCommand(string sqlStr)
		{
			Open();
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sqlStr;
			cmd.Connection = conn;
			return cmd;
		}

		private bool ExecuteNonQury(string sqlStr)
		{
			OleDbCommand cmd = CreateCommand(sqlStr);
			int result = cmd.ExecuteNonQuery();
			if (result == -1 | result == 0)
			{
				cmd.Dispose();
				Close();
				return false;
			}
			else
			{
				cmd.Dispose();
				Close();
				return true;
			}
		}

		public DataSet GetDataSet(string sqlStr)
		{
			DataSet ds = new DataSet();
			OleDbCommand cmd = CreateCommand(sqlStr);
			OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
			dataAdapter.Fill(ds);
			cmd.Dispose();
			Close();
			dataAdapter.Dispose();
			return ds;
		}

		public OleDbDataReader GetReader(string sqlStr)
		{
			OleDbCommand cmd = CreateCommand(sqlStr);
			OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			return reader;
		}
	}
}
