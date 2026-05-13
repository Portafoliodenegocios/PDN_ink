using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;


namespace HR_Templates.Proxys
{
    	public partial class  PDNInk_DocumentCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_DocumentCBR singlenton =null;
    
    
    		 public static PDNInk_DocumentCBR NewInstance()
    		 {
    			return  new PDNInk_DocumentCBR();
    		 }
    
    		 public static PDNInk_DocumentCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_DocumentCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_DocumentCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.Document>> GetAll()
             {
                PDNInk.DocumentResponse response = new PDNInk.DocumentResponse();
                PDNInk.DocumentRequest request = new PDNInk.DocumentRequest();
    
                try
                {
                    response = await client.Document_GetByAsync(request);
                }
                catch (FaultException<PDNInk.FaultInfo> ex)
                {
                    throw new Exception(ex.Message,ex.InnerException);
                }
    			return response.Items;
             }
    
    
    
    		 /// <summary>
    		 /// Consulta el registro que corresponde a la llave
    	     /// </summary>
    		 public async Task<PDNInk.Document> GetByKey(Int64? Document_Id)
             {
                PDNInk.DocumentResponse response = new PDNInk.DocumentResponse();
                PDNInk.DocumentRequest request = new PDNInk.DocumentRequest();
    
                try
                {
    				 request.Document_Id =  Document_Id; 
                    response = await client.Document_GetByKeyAsync(request);
                }
                catch (FaultException<PDNInk.FaultInfo> ex)
                {
                    throw new Exception(ex.Message,ex.InnerException);
                }
    			return response.Item;
             }
    
    
    
    
    
    		
    		 /// <summary>
    		 /// Realiza la consulta de acuerdo al predicado dado
    	     /// </summary>
    		 public async Task<List<PDNInk.Document>> GetBy(string predicate)
             {
                PDNInk.DocumentResponse response = new PDNInk.DocumentResponse();
                PDNInk.DocumentRequest request = new PDNInk.DocumentRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.Document_GetByAsync(request);
                }
                catch (FaultException<PDNInk.FaultInfo> ex)
                {
                    throw new Exception(ex.Message,ex.InnerException);
                }
    			return response.Items;
             }
    
        
    
    	     #endregion Get
             
    		 #region Actualize Data
    
    		 ///
    		 /// Crea un objeto
    		 ///
    		 public async Task<PDNInk.Document> Create(PDNInk.Document item)
             {
                PDNInk.DocumentResponse response = new PDNInk.DocumentResponse();
                PDNInk.DocumentRequest request = new PDNInk.DocumentRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.Document_CreateAsync(request);
                }
                catch (FaultException<PDNInk.FaultInfo> ex)
                {
                    throw new Exception(ex.Message,ex.InnerException);
                }
    			return response.Item;
             }
    
    
    		 ///
    		 /// Actualiza la información de un objeto
    		 ///
    		 public async Task<PDNInk.Document> Update(PDNInk.Document item)
             {
                PDNInk.DocumentResponse response = new PDNInk.DocumentResponse();
                PDNInk.DocumentRequest request = new PDNInk.DocumentRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.Document_UpdateAsync(request);
                }
                catch (FaultException<PDNInk.FaultInfo> ex)
                {
                    throw new Exception(ex.Message,ex.InnerException);
                }
    			return response.Item;
             }
    
    
    		 ///
    		 /// Elimina la información de un objeto
    		 ///
    		 public async Task<bool> Delete(PDNInk.Document item)
             {
                PDNInk.DocumentResponse response = new PDNInk.DocumentResponse();
                PDNInk.DocumentRequest request = new PDNInk.DocumentRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.Document_DeleteAsync(request);
                }
                catch (FaultException<PDNInk.FaultInfo> ex)
                {
                    throw new Exception(ex.Message,ex.InnerException);
                }
    			return response.Is_Delete;
             }
    		 #endregion Actualize Date
    
      }  
    
    
        }
