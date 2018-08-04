﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LiteDB
{
    public partial class LiteCollection<T>
    {
        /// <summary>
        /// Return a new LiteQueryable to build more complex queries
        /// </summary>
        public LiteQueryable<T> Query()
        {
            return new LiteQueryable<T>(_engine.Value.Query(_collection), _mapper);
        }

        #region Find

        /// <summary>
        /// Find documents inside a collection using predicate expression.
        /// </summary>
        public IEnumerable<T> Find(BsonExpression predicate, int skip = 0, int limit = int.MaxValue)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return this.Query()
                .Include(_includes)
                .Where(predicate)
                .Skip(skip)
                .Limit(limit)
                .ToEnumerable();
        }

        /// <summary>
        /// Find documents inside a collection using predicate expression.
        /// </summary>
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int skip = 0, int limit = int.MaxValue) => this.Find(_mapper.GetExpression(predicate), skip, limit);

        #endregion

        #region FindById + One + All

        /// <summary>
        /// Find a document using Document Id. Returns null if not found.
        /// </summary>
        public T FindById(BsonValue id)
        {
            if (id == null || id.IsNull) throw new ArgumentNullException(nameof(id));

            return this.Find(BsonExpression.Create("_id = @0", id)).FirstOrDefault();
        }

        /// <summary>
        /// Find the first document using predicate expression. Returns null if not found
        /// </summary>
        public T FindOne(BsonExpression predicate) => this.Find(predicate).FirstOrDefault();

        /// <summary>
        /// Find the first document using predicate expression. Returns null if not found
        /// </summary>
        public T FindOne(string predicate, BsonDocument parameters) => this.FindOne(BsonExpression.Create(predicate, parameters));

        /// <summary>
        /// Find the first document using predicate expression. Returns null if not found
        /// </summary>
        public T FindOne(BsonExpression predicate, params BsonValue[] args) => this.FindOne(BsonExpression.Create(predicate, args));

        /// <summary>
        /// Find the first document using predicate expression. Returns null if not found
        /// </summary>
        public T FindOne(Expression<Func<T, bool>> predicate) => this.FindOne(_mapper.GetExpression(predicate));

        /// <summary>
        /// Returns all documents inside collection order by _id index.
        /// </summary>
        public IEnumerable<T> FindAll() => this.Query().ToEnumerable();

        #endregion
    }
}