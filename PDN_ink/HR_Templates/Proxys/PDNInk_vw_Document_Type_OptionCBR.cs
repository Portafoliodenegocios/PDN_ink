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
    	public partial class  PDNInk_vw_Document_Type_OptionCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_vw_Document_Type_OptionCBR singlenton =null;
    
    
    		 public static PDNInk_vw_Document_Type_OptionCBR NewInstance()
    		 {
    			return  new PDNInk_vw_Document_Type_OptionCBR();
    		 }
    
    		 public static PDNInk_vw_Document_Type_OptionCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_vw_Document_Type_OptionCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_vw_Document_Type_OptionCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.vw_Document_Type_Option>> GetAll()
             {
                PDNInk.vw_Document_Type_OptionResponse response = new PDNInk.vw_Document_Type_OptionResponse();
                PDNInk.vw_Document_Type_OptionRequest request = new PDNInk.vw_Document_Type_OptionRequest();
    
                try
                {
                    response = await client.vw_Document_Type_Option_GetByAsync(request);
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
    		 public async Task<PDNInk.vw_Document_Type_Option> GetByKey(Int16? Document_Option_Id)
             {
                PDNInk.vw_Document_Type_OptionResponse response = new PDNInk.vw_Document_Type_OptionResponse();
                PDNInk.vw_Document_Type_OptionRequest request = new PDNInk.vw_Document_Type_OptionRequest();
    
                try
                {
    				 request.Document_Option_Id =  Document_Option_Id; 
                    response = await client.vw_Document_Type_Option_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.vw_Document_Type_Option>> GetBy(string predicate)
             {
                PDNInk.vw_Document_Type_OptionResponse response = new PDNInk.vw_Document_Type_OptionResponse();
                PDNInk.vw_Document_Type_OptionRequest request = new PDNInk.vw_Document_Type_OptionRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.vw_Document_Type_Option_GetByAsync(request);
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
    		 public async Task<PDNInk.vw_Document_Type_Option> Create(PDNInk.vw_Document_Type_Option item)
             {
                PDNInk.vw_Document_Type_OptionResponse response = new PDNInk.vw_Document_Type_OptionResponse();
                PDNInk.vw_Document_Type_OptionRequest request = new PDNInk.vw_Document_Type_OptionRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Document_Type_Option_CreateAsync(request);
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
    		 public async Task<PDNInk.vw_Document_Type_Option> Update(PDNInk.vw_Document_Type_Option item)
             {
                PDNInk.vw_Document_Type_OptionResponse response = new PDNInk.vw_Document_Type_OptionResponse();
                PDNInk.vw_Document_Type_OptionRequest request = new PDNInk.vw_Document_Type_OptionRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Document_Type_Option_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.vw_Document_Type_Option item)
             {
                PDNInk.vw_Document_Type_OptionResponse response = new PDNInk.vw_Document_Type_OptionResponse();
                PDNInk.vw_Document_Type_OptionRequest request = new PDNInk.vw_Document_Type_OptionRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.vw_Document_Type_Option_DeleteAsync(request);
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
