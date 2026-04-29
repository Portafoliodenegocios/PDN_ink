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
    	public partial class  PDNInk_cat_Signature_StatusCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_cat_Signature_StatusCBR singlenton =null;
    
    
    		 public static PDNInk_cat_Signature_StatusCBR NewInstance()
    		 {
    			return  new PDNInk_cat_Signature_StatusCBR();
    		 }
    
    		 public static PDNInk_cat_Signature_StatusCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_cat_Signature_StatusCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_cat_Signature_StatusCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.cat_Signature_Status>> GetAll()
             {
                PDNInk.cat_Signature_StatusResponse response = new PDNInk.cat_Signature_StatusResponse();
                PDNInk.cat_Signature_StatusRequest request = new PDNInk.cat_Signature_StatusRequest();
    
                try
                {
                    response = await client.cat_Signature_Status_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Signature_Status> GetByKey(Byte? Signature_Status_Id)
             {
                PDNInk.cat_Signature_StatusResponse response = new PDNInk.cat_Signature_StatusResponse();
                PDNInk.cat_Signature_StatusRequest request = new PDNInk.cat_Signature_StatusRequest();
    
                try
                {
    				 request.Signature_Status_Id =  Signature_Status_Id; 
                    response = await client.cat_Signature_Status_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.cat_Signature_Status>> GetBy(string predicate)
             {
                PDNInk.cat_Signature_StatusResponse response = new PDNInk.cat_Signature_StatusResponse();
                PDNInk.cat_Signature_StatusRequest request = new PDNInk.cat_Signature_StatusRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.cat_Signature_Status_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Signature_Status> Create(PDNInk.cat_Signature_Status item)
             {
                PDNInk.cat_Signature_StatusResponse response = new PDNInk.cat_Signature_StatusResponse();
                PDNInk.cat_Signature_StatusRequest request = new PDNInk.cat_Signature_StatusRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Signature_Status_CreateAsync(request);
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
    		 public async Task<PDNInk.cat_Signature_Status> Update(PDNInk.cat_Signature_Status item)
             {
                PDNInk.cat_Signature_StatusResponse response = new PDNInk.cat_Signature_StatusResponse();
                PDNInk.cat_Signature_StatusRequest request = new PDNInk.cat_Signature_StatusRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Signature_Status_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.cat_Signature_Status item)
             {
                PDNInk.cat_Signature_StatusResponse response = new PDNInk.cat_Signature_StatusResponse();
                PDNInk.cat_Signature_StatusRequest request = new PDNInk.cat_Signature_StatusRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Signature_Status_DeleteAsync(request);
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
