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
    	public partial class  PDNInk_Document_UserCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_Document_UserCBR singlenton =null;
    
    
    		 public static PDNInk_Document_UserCBR NewInstance()
    		 {
    			return  new PDNInk_Document_UserCBR();
    		 }
    
    		 public static PDNInk_Document_UserCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_Document_UserCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_Document_UserCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.Document_User>> GetAll()
             {
                PDNInk.Document_UserResponse response = new PDNInk.Document_UserResponse();
                PDNInk.Document_UserRequest request = new PDNInk.Document_UserRequest();
    
                try
                {
                    response = await client.Document_User_GetByAsync(request);
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
    		 public async Task<PDNInk.Document_User> GetByKey(Int64? Document_User_Id)
             {
                PDNInk.Document_UserResponse response = new PDNInk.Document_UserResponse();
                PDNInk.Document_UserRequest request = new PDNInk.Document_UserRequest();
    
                try
                {
    				 request.Document_User_Id =  Document_User_Id; 
                    response = await client.Document_User_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.Document_User>> GetBy(string predicate)
             {
                PDNInk.Document_UserResponse response = new PDNInk.Document_UserResponse();
                PDNInk.Document_UserRequest request = new PDNInk.Document_UserRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.Document_User_GetByAsync(request);
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
    		 public async Task<PDNInk.Document_User> Create(PDNInk.Document_User item)
             {
                PDNInk.Document_UserResponse response = new PDNInk.Document_UserResponse();
                PDNInk.Document_UserRequest request = new PDNInk.Document_UserRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.Document_User_CreateAsync(request);
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
    		 public async Task<PDNInk.Document_User> Update(PDNInk.Document_User item)
             {
                PDNInk.Document_UserResponse response = new PDNInk.Document_UserResponse();
                PDNInk.Document_UserRequest request = new PDNInk.Document_UserRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.Document_User_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.Document_User item)
             {
                PDNInk.Document_UserResponse response = new PDNInk.Document_UserResponse();
                PDNInk.Document_UserRequest request = new PDNInk.Document_UserRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.Document_User_DeleteAsync(request);
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
