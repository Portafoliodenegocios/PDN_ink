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
    	public partial class  PDNInk_cat_Document_ClassificationCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_cat_Document_ClassificationCBR singlenton =null;
    
    
    		 public static PDNInk_cat_Document_ClassificationCBR NewInstance()
    		 {
    			return  new PDNInk_cat_Document_ClassificationCBR();
    		 }
    
    		 public static PDNInk_cat_Document_ClassificationCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_cat_Document_ClassificationCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_cat_Document_ClassificationCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.cat_Document_Classification>> GetAll()
             {
                PDNInk.cat_Document_ClassificationResponse response = new PDNInk.cat_Document_ClassificationResponse();
                PDNInk.cat_Document_ClassificationRequest request = new PDNInk.cat_Document_ClassificationRequest();
    
                try
                {
                    response = await client.cat_Document_Classification_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Document_Classification> GetByKey(Int16? Document_Classification_Id)
             {
                PDNInk.cat_Document_ClassificationResponse response = new PDNInk.cat_Document_ClassificationResponse();
                PDNInk.cat_Document_ClassificationRequest request = new PDNInk.cat_Document_ClassificationRequest();
    
                try
                {
    				 request.Document_Classification_Id =  Document_Classification_Id; 
                    response = await client.cat_Document_Classification_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.cat_Document_Classification>> GetBy(string predicate)
             {
                PDNInk.cat_Document_ClassificationResponse response = new PDNInk.cat_Document_ClassificationResponse();
                PDNInk.cat_Document_ClassificationRequest request = new PDNInk.cat_Document_ClassificationRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.cat_Document_Classification_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Document_Classification> Create(PDNInk.cat_Document_Classification item)
             {
                PDNInk.cat_Document_ClassificationResponse response = new PDNInk.cat_Document_ClassificationResponse();
                PDNInk.cat_Document_ClassificationRequest request = new PDNInk.cat_Document_ClassificationRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Document_Classification_CreateAsync(request);
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
    		 public async Task<PDNInk.cat_Document_Classification> Update(PDNInk.cat_Document_Classification item)
             {
                PDNInk.cat_Document_ClassificationResponse response = new PDNInk.cat_Document_ClassificationResponse();
                PDNInk.cat_Document_ClassificationRequest request = new PDNInk.cat_Document_ClassificationRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Document_Classification_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.cat_Document_Classification item)
             {
                PDNInk.cat_Document_ClassificationResponse response = new PDNInk.cat_Document_ClassificationResponse();
                PDNInk.cat_Document_ClassificationRequest request = new PDNInk.cat_Document_ClassificationRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Document_Classification_DeleteAsync(request);
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
