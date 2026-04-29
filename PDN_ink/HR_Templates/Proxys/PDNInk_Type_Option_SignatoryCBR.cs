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
    	public partial class  PDNInk_Type_Option_SignatoryCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_Type_Option_SignatoryCBR singlenton =null;
    
    
    		 public static PDNInk_Type_Option_SignatoryCBR NewInstance()
    		 {
    			return  new PDNInk_Type_Option_SignatoryCBR();
    		 }
    
    		 public static PDNInk_Type_Option_SignatoryCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_Type_Option_SignatoryCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_Type_Option_SignatoryCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.Type_Option_Signatory>> GetAll()
             {
                PDNInk.Type_Option_SignatoryResponse response = new PDNInk.Type_Option_SignatoryResponse();
                PDNInk.Type_Option_SignatoryRequest request = new PDNInk.Type_Option_SignatoryRequest();
    
                try
                {
                    response = await client.Type_Option_Signatory_GetByAsync(request);
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
    		 public async Task<PDNInk.Type_Option_Signatory> GetByKey(Int16? Type_Option_Signatory_Id)
             {
                PDNInk.Type_Option_SignatoryResponse response = new PDNInk.Type_Option_SignatoryResponse();
                PDNInk.Type_Option_SignatoryRequest request = new PDNInk.Type_Option_SignatoryRequest();
    
                try
                {
    				 request.Type_Option_Signatory_Id =  Type_Option_Signatory_Id; 
                    response = await client.Type_Option_Signatory_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.Type_Option_Signatory>> GetBy(string predicate)
             {
                PDNInk.Type_Option_SignatoryResponse response = new PDNInk.Type_Option_SignatoryResponse();
                PDNInk.Type_Option_SignatoryRequest request = new PDNInk.Type_Option_SignatoryRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.Type_Option_Signatory_GetByAsync(request);
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
    		 public async Task<PDNInk.Type_Option_Signatory> Create(PDNInk.Type_Option_Signatory item)
             {
                PDNInk.Type_Option_SignatoryResponse response = new PDNInk.Type_Option_SignatoryResponse();
                PDNInk.Type_Option_SignatoryRequest request = new PDNInk.Type_Option_SignatoryRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.Type_Option_Signatory_CreateAsync(request);
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
    		 public async Task<PDNInk.Type_Option_Signatory> Update(PDNInk.Type_Option_Signatory item)
             {
                PDNInk.Type_Option_SignatoryResponse response = new PDNInk.Type_Option_SignatoryResponse();
                PDNInk.Type_Option_SignatoryRequest request = new PDNInk.Type_Option_SignatoryRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.Type_Option_Signatory_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.Type_Option_Signatory item)
             {
                PDNInk.Type_Option_SignatoryResponse response = new PDNInk.Type_Option_SignatoryResponse();
                PDNInk.Type_Option_SignatoryRequest request = new PDNInk.Type_Option_SignatoryRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.Type_Option_Signatory_DeleteAsync(request);
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
