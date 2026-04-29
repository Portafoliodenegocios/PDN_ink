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
    	public partial class  PDNInk_cat_Signature_SystemCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_cat_Signature_SystemCBR singlenton =null;
    
    
    		 public static PDNInk_cat_Signature_SystemCBR NewInstance()
    		 {
    			return  new PDNInk_cat_Signature_SystemCBR();
    		 }
    
    		 public static PDNInk_cat_Signature_SystemCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_cat_Signature_SystemCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_cat_Signature_SystemCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.cat_Signature_System>> GetAll()
             {
                PDNInk.cat_Signature_SystemResponse response = new PDNInk.cat_Signature_SystemResponse();
                PDNInk.cat_Signature_SystemRequest request = new PDNInk.cat_Signature_SystemRequest();
    
                try
                {
                    response = await client.cat_Signature_System_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Signature_System> GetByKey(Byte? Signature_System_Id)
             {
                PDNInk.cat_Signature_SystemResponse response = new PDNInk.cat_Signature_SystemResponse();
                PDNInk.cat_Signature_SystemRequest request = new PDNInk.cat_Signature_SystemRequest();
    
                try
                {
    				 request.Signature_System_Id =  Signature_System_Id; 
                    response = await client.cat_Signature_System_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.cat_Signature_System>> GetBy(string predicate)
             {
                PDNInk.cat_Signature_SystemResponse response = new PDNInk.cat_Signature_SystemResponse();
                PDNInk.cat_Signature_SystemRequest request = new PDNInk.cat_Signature_SystemRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.cat_Signature_System_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Signature_System> Create(PDNInk.cat_Signature_System item)
             {
                PDNInk.cat_Signature_SystemResponse response = new PDNInk.cat_Signature_SystemResponse();
                PDNInk.cat_Signature_SystemRequest request = new PDNInk.cat_Signature_SystemRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Signature_System_CreateAsync(request);
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
    		 public async Task<PDNInk.cat_Signature_System> Update(PDNInk.cat_Signature_System item)
             {
                PDNInk.cat_Signature_SystemResponse response = new PDNInk.cat_Signature_SystemResponse();
                PDNInk.cat_Signature_SystemRequest request = new PDNInk.cat_Signature_SystemRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Signature_System_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.cat_Signature_System item)
             {
                PDNInk.cat_Signature_SystemResponse response = new PDNInk.cat_Signature_SystemResponse();
                PDNInk.cat_Signature_SystemRequest request = new PDNInk.cat_Signature_SystemRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Signature_System_DeleteAsync(request);
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
