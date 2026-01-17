using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Persistence.Context;
using Inventory.Persistence.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Persistence.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = new();
        private IDbContextTransaction? _transaction;

        public UnitOfWork(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T> Repositories<T>() where T : class
        {
            var typeName = typeof(T).FullName!;
            if (!_repositories.ContainsKey(typeName))
            {
                var repositoryInstance = new GenericRepository<T>(_dbContext);
                _repositories.Add(typeName, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[typeName];
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }


        //  Begin transaction
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
                _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        //  Commit transaction
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                if (_transaction != null)
                    await _transaction.CommitAsync();
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        //Rollback transaction
        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                    await _transaction.RollbackAsync();
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
            if (_transaction != null)
                await _transaction.DisposeAsync();
        }


        public void PrintEntityStates()
        {
            Console.WriteLine("------ Entity States ------");
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
            }
            Console.WriteLine("---------------------------");
        }
    }
}
