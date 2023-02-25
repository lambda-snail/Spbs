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
    /// Obtain JWT pair.
    /// </summary>
    [DataContract(Name = "JWTObtainPairRequest")]
    public partial class JWTObtainPairRequest : IEquatable<JWTObtainPairRequest>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JWTObtainPairRequest" /> class.
        /// </summary>
        [JsonConstructor]
        protected JWTObtainPairRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="JWTObtainPairRequest" /> class.
        /// </summary>
        /// <param name="secretId">Secret id from /user-secrets/ (required).</param>
        /// <param name="secretKey">Secret key from /user-secrets/ (required).</param>
        public JWTObtainPairRequest(string secretId = default(string), string secretKey = default(string))
        {
            // to ensure "secretId" is required (not null)
            if (secretId == null)
            {
                throw new ArgumentNullException("secretId is a required property for JWTObtainPairRequest and cannot be null");
            }
            this.SecretId = secretId;
            // to ensure "secretKey" is required (not null)
            if (secretKey == null)
            {
                throw new ArgumentNullException("secretKey is a required property for JWTObtainPairRequest and cannot be null");
            }
            this.SecretKey = secretKey;
        }

        /// <summary>
        /// Secret id from /user-secrets/
        /// </summary>
        /// <value>Secret id from /user-secrets/</value>
        [DataMember(Name = "secret_id", IsRequired = true, EmitDefaultValue = true)]
        public string SecretId { get; set; }

        /// <summary>
        /// Secret key from /user-secrets/
        /// </summary>
        /// <value>Secret key from /user-secrets/</value>
        [DataMember(Name = "secret_key", IsRequired = true, EmitDefaultValue = true)]
        public string SecretKey { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class JWTObtainPairRequest {\n");
            sb.Append("  SecretId: ").Append(SecretId).Append("\n");
            sb.Append("  SecretKey: ").Append(SecretKey).Append("\n");
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
            return this.Equals(input as JWTObtainPairRequest);
        }

        /// <summary>
        /// Returns true if JWTObtainPairRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of JWTObtainPairRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JWTObtainPairRequest input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.SecretId == input.SecretId ||
                    (this.SecretId != null &&
                    this.SecretId.Equals(input.SecretId))
                ) && 
                (
                    this.SecretKey == input.SecretKey ||
                    (this.SecretKey != null &&
                    this.SecretKey.Equals(input.SecretKey))
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
                if (this.SecretId != null)
                {
                    hashCode = (hashCode * 59) + this.SecretId.GetHashCode();
                }
                if (this.SecretKey != null)
                {
                    hashCode = (hashCode * 59) + this.SecretKey.GetHashCode();
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