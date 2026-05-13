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
    	public partial class  PDNInk_vw_Document_User_FolderCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_vw_Document_User_FolderCBR singlenton =null;
    
    
    		 public static PDNInk_vw_Document_User_FolderCBR NewInstance()
    		 {
    			return  new PDNInk_vw_Document_User_FolderCBR();
    		 }
    
    		 public static PDNInk_vw_Document_User_FolderCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_vw_Document_User_FolderCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_vw_Document_User_FolderCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.vw_Document_User_Folder>> GetAll()
             {
                PDNInk.vw_Document_User_FolderResponse response = new PDNInk.vw_Document_User_FolderResponse();
                PDNInk.vw_Document_User_FolderRequest request = new PDNInk.vw_Document_User_FolderRequest();
    
                try
                {
                    response = await client.vw_Document_User_Folder_GetByAsync(request);
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
    		 public async Task<PDNInk.vw_Document_User_Folder> GetByKey(Int64? Document_User_Folder_Id)
             {
                PDNInk.vw_Document_User_FolderResponse response = new PDNInk.vw_Document_User_FolderResponse();
                PDNInk.vw_Document_User_FolderRequest request = new PDNInk.vw_Document_User_FolderRequest();
    
                try
                {
    				 request.Document_User_Folder_Id =  Document_User_Folder_Id; 
                    response = await client.vw_Document_User_Folder_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.vw_Document_User_Folder>> GetBy(string predicate)
             {
                PDNInk.vw_Document_User_FolderResponse response = new PDNInk.vw_Document_User_FolderResponse();
                PDNInk.vw_Document_User_FolderRequest request = new PDNInk.vw_Document_User_FolderRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.vw_Document_User_Folder_GetByAsync(request);
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
    		 public async Task<PDNInk.vw_Document_User_Folder> Create(PDNInk.vw_Document_User_Folder item)
             {
                PDNInk.vw_Document_User_FolderResponse response = new PDNInk.vw_Document_User_FolderResponse();
                PDNInk.vw_Document_User_FolderRequest request = new PDNInk.vw_Document_User_FolderRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Document_User_Folder_CreateAsync(request);
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
    		 public async Task<PDNInk.vw_Document_User_Folder> Update(PDNInk.vw_Document_User_Folder item)
             {
                PDNInk.vw_Document_User_FolderResponse response = new PDNInk.vw_Document_User_FolderResponse();
                PDNInk.vw_Document_User_FolderRequest request = new PDNInk.vw_Document_User_FolderRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Document_User_Folder_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.vw_Document_User_Folder item)
             {
                PDNInk.vw_Document_User_FolderResponse response = new PDNInk.vw_Document_User_FolderResponse();
                PDNInk.vw_Document_User_FolderRequest request = new PDNInk.vw_Document_User_FolderRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Document_User_Folder_DeleteAsync(request);
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
