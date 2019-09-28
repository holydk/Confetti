using System;
using System.Data;
using System.Data.Common;

namespace Confetti.Data.Services
{
    /// <summary>
    /// Represents a extensions for <see cref="IDataProviderHandler"/>.
    /// </summary>
    public static class DataProviderExtensions
    {
        #region Utilities

        /// <summary>
        /// Gets a db parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="dbType">The data type of a field, a property, or a Parameter object.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns>The parameter.</returns>
        private static DbParameter GetParameter(this IDataProviderHandler dataProvider, DbType dbType, string parameterName, object parameterValue)
        {
            var parameter = dataProvider.GetParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.DbType = dbType;

            return parameter;
        }

        /// <summary>
        /// Gets a output db parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="dbType">The data type of a field, a property, or a Parameter object.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The parameter.</returns>
        private static DbParameter GetOutputParameter(this IDataProviderHandler dataProvider, DbType dbType, string parameterName)
        {
            var parameter = dataProvider.GetParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Direction = ParameterDirection.Output;

            return parameter;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns>The parameter.</returns>
        public static DbParameter GetStringParameter(this IDataProviderHandler dataProvider, string parameterName, string parameterValue)
        {
            return dataProvider.GetParameter(DbType.String, parameterName, (object)parameterValue ?? DBNull.Value);
        }

        /// <summary>
        /// Gets a output string parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The parameter.</returns>
        public static DbParameter GetOutputStringParameter(this IDataProviderHandler dataProvider, string parameterName)
        {
            return dataProvider.GetOutputParameter(DbType.String, parameterName);
        }

        /// <summary>
        /// Gets a int parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns>The parameter.</returns>
        public static DbParameter GetInt32Parameter(this IDataProviderHandler dataProvider, string parameterName, int? parameterValue)
        {
            return dataProvider.GetParameter(DbType.Int32, parameterName, parameterValue.HasValue ? (object)parameterValue.Value : DBNull.Value);
        }
        
        /// <summary>
        /// Gets a output int32 parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The parameter.</returns>
        public static DbParameter GetOutputInt32Parameter(this IDataProviderHandler dataProvider, string parameterName)
        {
            return dataProvider.GetOutputParameter(DbType.Int32, parameterName);
        }

        /// <summary>
        /// Gets a output Decimal parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The parameter.</returns>
        public static DbParameter GetOutputDecimalParameter(this IDataProviderHandler dataProvider, string parameterName)
        {
            return dataProvider.GetOutputParameter(DbType.Decimal, parameterName);
        }

        /// <summary>
        /// Gets a boolean parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns>The parameter.</returns>
        public static DbParameter GetBooleanParameter(this IDataProviderHandler dataProvider, string parameterName, bool? parameterValue)
        {
            return dataProvider.GetParameter(DbType.Boolean, parameterName, parameterValue.HasValue ? (object)parameterValue.Value : DBNull.Value);
        }
        
        /// <summary>
        /// Gets a decimal parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns>The parameter.</returns>
        public static DbParameter GetDecimalParameter(this IDataProviderHandler dataProvider, string parameterName, decimal? parameterValue)
        {
            return dataProvider.GetParameter(DbType.Decimal, parameterName, parameterValue.HasValue ? (object)parameterValue.Value : DBNull.Value);
        }

        /// <summary>
        /// Gets a datetime parameter.
        /// </summary>
        /// <param name="dataProvider">The handler of the data provider.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns>The parameter.</returns>
        public static DbParameter GetDateTimeParameter(this IDataProviderHandler dataProvider, string parameterName, DateTime? parameterValue)
        {
            return dataProvider.GetParameter(DbType.DateTime, parameterName, parameterValue.HasValue ? (object)parameterValue.Value : DBNull.Value);
        }

        #endregion
    }
}