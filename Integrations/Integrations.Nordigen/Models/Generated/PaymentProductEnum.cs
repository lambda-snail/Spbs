/*
 * Nordigen Account Information Services API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 2.0 (v2)
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Integrations.Nordigen.Models
{
    /// <summary>
    /// Defines PaymentProductEnum
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentProductEnum
    {
        /// <summary>
        /// Enum T2P for value: T2P
        /// </summary>
        [EnumMember(Value = "T2P")]
        T2P = 1,

        /// <summary>
        /// Enum SCT for value: SCT
        /// </summary>
        [EnumMember(Value = "SCT")]
        SCT = 2,

        /// <summary>
        /// Enum ISCT for value: ISCT
        /// </summary>
        [EnumMember(Value = "ISCT")]
        ISCT = 3,

        /// <summary>
        /// Enum CBCT for value: CBCT
        /// </summary>
        [EnumMember(Value = "CBCT")]
        CBCT = 4

    }

}
