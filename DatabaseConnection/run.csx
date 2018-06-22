#r "System.Data"

using System;
using System.Net; 
using System.Data.SqlClient;
using System.Data;

private static SqlConnection connection = new SqlConnection();

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log){    
    
    log.Info("C# HTTP trigger function processed a request.");

    try
    {
        if(connection.State.Equals(ConnectionState.Closed)){
            connection.ConnectionString = "";
        await connection.OpenAsync();

        using (SqlCommand command = new SqlCommand("SELECT * FROM SYSOBJECTS WHERE xtype = 'U' ORDER BY 1", connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    log.Info($"{reader.GetString(0)}");
                }
            }

        }
        
        return req.CreateResponse(HttpStatusCode.OK, 
            $"The database connection is: {connection.State}");
    }
    catch (SqlException sqlex)
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, 
            $"The following SqlException happened: {sqlex.Message}");
    }
    catch (Exception ex)
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, 
            $"The following Exception happened: {ex.Message}");
    }
}
