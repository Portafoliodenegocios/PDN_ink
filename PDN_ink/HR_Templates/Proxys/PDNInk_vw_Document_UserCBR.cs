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
    	public partial class  PDNInk_vw_Document_UserCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_vw_Document_UserCBR singlenton =null;
    
    
    		 public static PDNInk_vw_Document_UserCBR NewInstance()
    		 {
    			return  new PDNInk_vw_Document_UserCBR();
    		 }
    
    		 public static PDNInk_vw_Document_UserCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_vw_Document_UserCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_vw_Document_UserCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.vw_Document_User>> GetAll()
             {
                PDNInk.vw_Document_UserResponse response = new PDNInk.vw_Document_UserResponse();
                PDNInk.vw_Document_UserRequest request = new PDNInk.vw_Document_UserRequest();
    
                try
                {
                    response = await client.vw_Document_User_GetByAsync(request);
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
    		 public async Task<PDNInk.vw_Document_User> GetByKey(String Signature_Status)
             {
                PDNInk.vw_Document_UserResponse response = new PDNInk.vw_Document_UserResponse();
                PDNInk.vw_Document_UserRequest request = new PDNInk.vw_Document_UserRequest();
    
                try
                {
    				 request.Signature_Status =  Signature_Status; 
                    response = await client.vw_Document_User_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.vw_Document_User>> GetBy(string predicate)
             {
                PDNInk.vw_Document_UserResponse response = new PDNInk.vw_Document_UserResponse();
                PDNInk.vw_Document_UserRequest request = new PDNInk.vw_Document_UserRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.vw_Document_User_GetByAsync(request);
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
    		 public async Task<PDNInk.vw_Document_User> Create(PDNInk.vw_Document_User item)
             {
                PDNInk.vw_Document_UserResponse response = new PDNInk.vw_Document_UserResponse();
                PDNInk.vw_Document_UserRequest request = new PDNInk.vw_Document_UserRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Document_User_CreateAsync(request);
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
    		 public async Task<PDNInk.vw_Document_User> Update(PDNInk.vw_Document_User item)
             {
                PDNInk.vw_Document_UserResponse response = new PDNInk.vw_Document_UserResponse();
                PDNInk.vw_Document_UserRequest request = new PDNInk.vw_Document_UserRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Document_User_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.vw_Document_User item)
             {
                PDNInk.vw_Document_UserResponse response = new PDNInk.vw_Document_UserResponse();
                PDNInk.vw_Document_UserRequest request = new PDNInk.vw_Document_UserRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Document_User_DeleteAsync(request);
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
