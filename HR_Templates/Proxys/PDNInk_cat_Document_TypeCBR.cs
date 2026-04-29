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
    	public partial class  PDNInk_cat_Document_TypeCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_cat_Document_TypeCBR singlenton =null;
    
    
    		 public static PDNInk_cat_Document_TypeCBR NewInstance()
    		 {
    			return  new PDNInk_cat_Document_TypeCBR();
    		 }
    
    		 public static PDNInk_cat_Document_TypeCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_cat_Document_TypeCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_cat_Document_TypeCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.cat_Document_Type>> GetAll()
             {
                PDNInk.cat_Document_TypeResponse response = new PDNInk.cat_Document_TypeResponse();
                PDNInk.cat_Document_TypeRequest request = new PDNInk.cat_Document_TypeRequest();
    
                try
                {
                    response = await client.cat_Document_Type_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Document_Type> GetByKey(Int16? Document_Type_Id)
             {
                PDNInk.cat_Document_TypeResponse response = new PDNInk.cat_Document_TypeResponse();
                PDNInk.cat_Document_TypeRequest request = new PDNInk.cat_Document_TypeRequest();
    
                try
                {
    				 request.Document_Type_Id =  Document_Type_Id; 
                    response = await client.cat_Document_Type_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.cat_Document_Type>> GetBy(string predicate)
             {
                PDNInk.cat_Document_TypeResponse response = new PDNInk.cat_Document_TypeResponse();
                PDNInk.cat_Document_TypeRequest request = new PDNInk.cat_Document_TypeRequest();

           
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.cat_Document_Type_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Document_Type> Create(PDNInk.cat_Document_Type item)
             {
                PDNInk.cat_Document_TypeResponse response = new PDNInk.cat_Document_TypeResponse();
                PDNInk.cat_Document_TypeRequest request = new PDNInk.cat_Document_TypeRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Document_Type_CreateAsync(request);
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
    		 public async Task<PDNInk.cat_Document_Type> Update(PDNInk.cat_Document_Type item)
             {
                PDNInk.cat_Document_TypeResponse response = new PDNInk.cat_Document_TypeResponse();
                PDNInk.cat_Document_TypeRequest request = new PDNInk.cat_Document_TypeRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Document_Type_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.cat_Document_Type item)
             {
                PDNInk.cat_Document_TypeResponse response = new PDNInk.cat_Document_TypeResponse();
                PDNInk.cat_Document_TypeRequest request = new PDNInk.cat_Document_TypeRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Document_Type_DeleteAsync(request);
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
