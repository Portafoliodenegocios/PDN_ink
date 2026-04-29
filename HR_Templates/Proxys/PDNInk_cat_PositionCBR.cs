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
    	public partial class  PDNInk_cat_PositionCBR
    	{
    
             private static PDNInk.PDNInkClient client; 
       		 private static PDNInk_cat_PositionCBR singlenton =null;
    
    
    		 public static PDNInk_cat_PositionCBR NewInstance()
    		 {
    			return  new PDNInk_cat_PositionCBR();
    		 }
    
    		 public static PDNInk_cat_PositionCBR Instance
    		 {
    		 	 get
    			 {
       				 if (singlenton == null)
       					 singlenton = new PDNInk_cat_PositionCBR();
       				 return singlenton;
    			 }
    		 }
    
    
             #region Constructor
             public PDNInk_cat_PositionCBR()
             {
                client = new PDNInk.PDNInkClient();
             }
             #endregion
    
    	     #region Get
    
    		 /// <summary>
    		 /// Consulta todos los registros de la entidad 
    	     /// </summary>
    		 public async Task<List<PDNInk.cat_Position>> GetAll()
             {
                PDNInk.cat_PositionResponse response = new PDNInk.cat_PositionResponse();
                PDNInk.cat_PositionRequest request = new PDNInk.cat_PositionRequest();
    
                try
                {
                    response = await client.cat_Position_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Position> GetByKey(Int16? Position_Id)
             {
                PDNInk.cat_PositionResponse response = new PDNInk.cat_PositionResponse();
                PDNInk.cat_PositionRequest request = new PDNInk.cat_PositionRequest();
    
                try
                {
    				 request.Position_Id =  Position_Id; 
                    response = await client.cat_Position_GetByKeyAsync(request);
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
    		 public async Task<List<PDNInk.cat_Position>> GetBy(string predicate)
             {
                PDNInk.cat_PositionResponse response = new PDNInk.cat_PositionResponse();
                PDNInk.cat_PositionRequest request = new PDNInk.cat_PositionRequest();
    
                try
                {
    				request.Predicate = predicate;
    				request.Includes = string.Empty;
                    response = await client.cat_Position_GetByAsync(request);
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
    		 public async Task<PDNInk.cat_Position> Create(PDNInk.cat_Position item)
             {
                PDNInk.cat_PositionResponse response = new PDNInk.cat_PositionResponse();
                PDNInk.cat_PositionRequest request = new PDNInk.cat_PositionRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Position_CreateAsync(request);
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
    		 public async Task<PDNInk.cat_Position> Update(PDNInk.cat_Position item)
             {
                PDNInk.cat_PositionResponse response = new PDNInk.cat_PositionResponse();
                PDNInk.cat_PositionRequest request = new PDNInk.cat_PositionRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Position_UpdateAsync(request);
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
    		 public async Task<bool> Delete(PDNInk.cat_Position item)
             {
                PDNInk.cat_PositionResponse response = new PDNInk.cat_PositionResponse();
                PDNInk.cat_PositionRequest request = new PDNInk.cat_PositionRequest();
    
                try
                {
    				request.Item = item;
                    response = await client.cat_Position_DeleteAsync(request);
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
