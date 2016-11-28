using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.Models;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Data.Base.MSSql;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HIS.Recipes.Services.Implementation.Services
{
    public abstract class BaseService<T, TDbEntity, TViewModel, TCreationViewModel> : IDisposable
        where T : IRepositoryFindSingle<TDbEntity, Guid>, IRepositoryUpdate<TDbEntity, Guid>,
        IRepositoryAddAndDelete<TDbEntity, Guid>
        where TDbEntity : class, IEntity<Guid>
        where TViewModel : IViewModelEntity<Guid>
    {
        #region CONST

        #endregion

        #region FIELDS

        private bool _disposed;
        private readonly string _entityName;

        #endregion

        #region CTOR

        protected BaseService(T rep, IMapper mapper, ILogger logger, string entityName)
        {
            if (rep == null)
            {
                throw new ArgumentNullException(nameof(rep));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _entityName = entityName;
            Repository = rep;
            Mapper = mapper;
            Logger = logger;
        }

        ~BaseService()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Updates a database entity with the given data from a view model
        /// </summary>
        /// <param name="id">Database id</param>
        /// <param name="model">New Data</param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Guid id, TViewModel model)
        {
            try
            {
                if (id.Equals(Guid.Empty))
                {
                    throw new ArgumentNullException(nameof(id));
                }
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                if (id.Equals(model.Id))
                {
                    throw new IdsNotIdenticalException();
                }

                var existingElement = await this.Repository.FindAsync(id);
                var newObject = Mapper.Map(model, existingElement);
                var updateResult = await Repository.UpdateAsync(newObject);
                Logger.LogDebug(new EventId(),
                    updateResult ? $"{_entityName} {id} successfully updated" : $"No need to update step {id}");
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogWarning(new EventId(), e, $"No {_entityName} with id {id} found");
                throw new DataObjectNotFoundException();
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(), e, $"An Error occured while updating a {_entityName}");
                throw new Exception($"An Error occured while updating a {_entityName}");
            }
        }

        /// <summary>
        /// Creates a new entity in the Database
        /// </summary>
        /// <param name="creationModel">entity data</param>
        /// <returns></returns>
        public virtual async Task<TViewModel> AddAsync(TCreationViewModel creationModel)
        {
            TViewModel result;

            try
            {
                var datamodel = this.Mapper.Map<TDbEntity>(creationModel);
                await Repository.AddAsync(datamodel);
                result = this.Mapper.Map<TViewModel>(datamodel);
                Logger.LogDebug($"New {_entityName} '{datamodel.Id}' successfully created");
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(), e, $"An Error occured while creating a {_entityName}");
                throw new Exception($"An Error occured while creating a {_entityName}");
            }
            return result;
        }

        /// <summary>
        /// Deletes an entity from the Database
        /// </summary>
        /// <param name="id">entity id</param>
        /// <returns></returns>
        public virtual async Task RemoveAsync(Guid id)
        {
            try
            {
                var element = await this.Repository.FindAsync(id);
                if (element == null)
                {
                    throw new DataObjectNotFoundException();
                }
                await this.Repository.RemoveAsync(element);
                Logger.LogDebug($"{_entityName} '{id}' successfully deleted");
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(), e, $"An Error occured while deleting the {_entityName} '{id}'");
                throw new Exception($"An Error occured while deleting the {_entityName} '{id}'");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        /// <summary>
        /// Releases an returns unnessessary system resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region PROPERTIES
        protected IMapper Mapper { get; }
        protected ILogger Logger { get; }

        protected virtual T Repository { get; }

        #endregion

    }
}
