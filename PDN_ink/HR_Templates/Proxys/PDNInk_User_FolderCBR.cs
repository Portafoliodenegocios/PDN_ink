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
    	public partial class  PDNInk_User_FolderCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_User_FolderCBR singlenton =null;
    
    
    		 public static PDNInk_User_FolderCBR NewInstance()
    		 {
    			return  new PDNInk_User_FolderCBR();
    		 }
    
    		 public static PDNInk_User_FolderCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_User_FolderCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_User_FolderCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.User_Folder>> GetAll()
             {
                PDNInk.User_FolderResponse response = new PDNInk.User_FolderResponse();
                PDNInk.User_FolderRequest request = new PDNInk.User_FolderRequest();
    
                try
                {
                    response = await client.User_Folder_GetByAsync(request);
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
    		 public async Task<PDNInk.User_Folder> GetByKey(Int32? User_Folder_Id)
             {
                PDNInk.User_FolderResponse response = new PDNInk.User_FolderResponse();
                PDNInk.User_FolderRequest request = new PDNInk.User_FolderRequest();
    
                try
                {
    				 request.User_Folder_Id =  User_Folder_Id; 
                    response = await client.User_Folder_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.User_Folder>> GetBy(string predicate)
             {
                PDNInk.User_FolderResponse response = new PDNInk.User_FolderResponse();
                PDNInk.User_FolderRequest request = new PDNInk.User_FolderRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.User_Folder_GetByAsync(request);
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
    		 public async Task<PDNInk.User_Folder> Create(PDNInk.User_Folder item)
             {
                PDNInk.User_FolderResponse response = new PDNInk.User_FolderResponse();
                PDNInk.User_FolderRequest request = new PDNInk.User_FolderRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.User_Folder_CreateAsync(request);
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
    		 public async Task<PDNInk.User_Folder> Update(PDNInk.User_Folder item)
             {
                PDNInk.User_FolderResponse response = new PDNInk.User_FolderResponse();
                PDNInk.User_FolderRequest request = new PDNInk.User_FolderRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.User_Folder_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.User_Folder item)
             {
                PDNInk.User_FolderResponse response = new PDNInk.User_FolderResponse();
                PDNInk.User_FolderRequest request = new PDNInk.User_FolderRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.User_Folder_DeleteAsync(request);
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
