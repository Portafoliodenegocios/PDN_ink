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
    	public partial class  PDNInk_vw_DocumentsCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_vw_DocumentsCBR singlenton =null;
    
    
    		 public static PDNInk_vw_DocumentsCBR NewInstance()
    		 {
    			return  new PDNInk_vw_DocumentsCBR();
    		 }
    
    		 public static PDNInk_vw_DocumentsCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_vw_DocumentsCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_vw_DocumentsCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.vw_Documents>> GetAll()
             {
                PDNInk.vw_DocumentsResponse response = new PDNInk.vw_DocumentsResponse();
                PDNInk.vw_DocumentsRequest request = new PDNInk.vw_DocumentsRequest();
    
                try
                {
                    response = await client.vw_Documents_GetByAsync(request);
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
    		 public async Task<PDNInk.vw_Documents> GetByKey(Int64? Document_Id)
             {
                PDNInk.vw_DocumentsResponse response = new PDNInk.vw_DocumentsResponse();
                PDNInk.vw_DocumentsRequest request = new PDNInk.vw_DocumentsRequest();
    
                try
                {
    				 request.Document_Id =  Document_Id; 
                    response = await client.vw_Documents_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.vw_Documents>> GetBy(string predicate)
             {
                PDNInk.vw_DocumentsResponse response = new PDNInk.vw_DocumentsResponse();
                PDNInk.vw_DocumentsRequest request = new PDNInk.vw_DocumentsRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.vw_Documents_GetByAsync(request);
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
    		 public async Task<PDNInk.vw_Documents> Create(PDNInk.vw_Documents item)
             {
                PDNInk.vw_DocumentsResponse response = new PDNInk.vw_DocumentsResponse();
                PDNInk.vw_DocumentsRequest request = new PDNInk.vw_DocumentsRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Documents_CreateAsync(request);
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
    		 public async Task<PDNInk.vw_Documents> Update(PDNInk.vw_Documents item)
             {
                PDNInk.vw_DocumentsResponse response = new PDNInk.vw_DocumentsResponse();
                PDNInk.vw_DocumentsRequest request = new PDNInk.vw_DocumentsRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Documents_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.vw_Documents item)
             {
                PDNInk.vw_DocumentsResponse response = new PDNInk.vw_DocumentsResponse();
                PDNInk.vw_DocumentsRequest request = new PDNInk.vw_DocumentsRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Documents_DeleteAsync(request);
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
