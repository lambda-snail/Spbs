/*
 * Nordigen Account Information Services API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 2.0 (v2)
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Integrations.Nordigen.Models
{
    /// <summary>
    /// Represents end-user details.
    /// </summary>
    [DataContract(Name = "EnduserAcceptanceDetailsRequest")]
    public partial class EnduserAcceptanceDetailsRequest : IEquatable<EnduserAcceptanceDetailsRequest>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnduserAcceptanceDetailsRequest" /> class.
        /// </summary>
        [JsonConstructor]
        protected EnduserAcceptanceDetailsRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="EnduserAcceptanceDetailsRequest" /> class.
        /// </summary>
        /// <param name="userAgent">userAgent (required).</param>
        /// <param name="ipAddress">ipAddress (required).</param>
        public EnduserAcceptanceDetailsRequest(string userAgent = default(string), string ipAddress = default(string))
        {
            // to ensure "userAgent" is required (not null)
            if (userAgent == null)
            {
                throw new ArgumentNullException("userAgent is a required property for EnduserAcceptanceDetailsRequest and cannot be null");
            }
            this.UserAgent = userAgent;
            // to ensure "ipAddress" is required (not null)
            if (ipAddress == null)
            {
                throw new ArgumentNullException("ipAddress is a required property for EnduserAcceptanceDetailsRequest and cannot be null");
            }
            this.IpAddress = ipAddress;
        }

        /// <summary>
        /// Gets or Sets UserAgent
        /// </summary>
        [DataMember(Name = "user_agent", IsRequired = true, EmitDefaultValue = true)]
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or Sets IpAddress
        /// </summary>
        [DataMember(Name = "ip_address", IsRequired = true, EmitDefaultValue = true)]
        public string IpAddress { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class EnduserAcceptanceDetailsRequest {\n");
            sb.Append("  UserAgent: ").Append(UserAgent).Append("\n");
            sb.Append("  IpAddress: ").Append(IpAddress).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as EnduserAcceptanceDetailsRequest);
        }

        /// <summary>
        /// Returns true if EnduserAcceptanceDetailsRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of EnduserAcceptanceDetailsRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EnduserAcceptanceDetailsRequest input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.UserAgent == input.UserAgent ||
                    (this.UserAgent != null &&
                    this.UserAgent.Equals(input.UserAgent))
                ) && 
                (
                    this.IpAddress == input.IpAddress ||
                    (this.IpAddress != null &&
                    this.IpAddress.Equals(input.IpAddress))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.UserAgent != null)
                {
                    hashCode = (hashCode * 59) + this.UserAgent.GetHashCode();
                }
                if (this.IpAddress != null)
                {
                    hashCode = (hashCode * 59) + this.IpAddress.GetHashCode();
                }
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}