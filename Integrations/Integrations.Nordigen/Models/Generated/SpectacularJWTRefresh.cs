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
    /// Refresh Access token.
    /// </summary>
    [DataContract(Name = "SpectacularJWTRefresh")]
    public partial class SpectacularJWTRefresh : IEquatable<SpectacularJWTRefresh>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpectacularJWTRefresh" /> class.
        /// </summary>
        [JsonConstructor]
        public SpectacularJWTRefresh()
        {
        }

        /// <summary>
        /// Your access token
        /// </summary>
        /// <value>Your access token</value>
        [DataMember(Name = "access", EmitDefaultValue = false)]
        public string Access { get; private set; }

        /// <summary>
        /// Returns false as Access should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeAccess()
        {
            return false;
        }
        /// <summary>
        /// Access token expires in seconds
        /// </summary>
        /// <value>Access token expires in seconds</value>
        [DataMember(Name = "access_expires", EmitDefaultValue = false)]
        public int AccessExpires { get; private set; }

        /// <summary>
        /// Returns false as AccessExpires should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeAccessExpires()
        {
            return false;
        }
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class SpectacularJWTRefresh {\n");
            sb.Append("  Access: ").Append(Access).Append("\n");
            sb.Append("  AccessExpires: ").Append(AccessExpires).Append("\n");
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
            return this.Equals(input as SpectacularJWTRefresh);
        }

        /// <summary>
        /// Returns true if SpectacularJWTRefresh instances are equal
        /// </summary>
        /// <param name="input">Instance of SpectacularJWTRefresh to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SpectacularJWTRefresh input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Access == input.Access ||
                    (this.Access != null &&
                    this.Access.Equals(input.Access))
                ) && 
                (
                    this.AccessExpires == input.AccessExpires ||
                    this.AccessExpires.Equals(input.AccessExpires)
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
                if (this.Access != null)
                {
                    hashCode = (hashCode * 59) + this.Access.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.AccessExpires.GetHashCode();
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
