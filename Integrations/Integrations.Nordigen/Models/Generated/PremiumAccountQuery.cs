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
    /// Filter country.
    /// </summary>
    [DataContract(Name = "PremiumAccountQuery")]
    public partial class PremiumAccountQuery : IEquatable<PremiumAccountQuery>, IValidatableObject
    {

        /// <summary>
        /// Gets or Sets Country
        /// </summary>
        [DataMember(Name = "country", EmitDefaultValue = false)]
        public CountryEnum? Country { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PremiumAccountQuery" /> class.
        /// </summary>
        /// <param name="dateFrom">dateFrom.</param>
        /// <param name="dateTo">dateTo.</param>
        /// <param name="country">country.</param>
        public PremiumAccountQuery(DateTime dateFrom = default(DateTime), DateTime dateTo = default(DateTime), CountryEnum? country = default(CountryEnum?))
        {
            this.DateFrom = dateFrom;
            this.DateTo = dateTo;
            this.Country = country;
        }

        /// <summary>
        /// Gets or Sets DateFrom
        /// </summary>
        [DataMember(Name = "date_from", EmitDefaultValue = false)]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or Sets DateTo
        /// </summary>
        [DataMember(Name = "date_to", EmitDefaultValue = false)]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class PremiumAccountQuery {\n");
            sb.Append("  DateFrom: ").Append(DateFrom).Append("\n");
            sb.Append("  DateTo: ").Append(DateTo).Append("\n");
            sb.Append("  Country: ").Append(Country).Append("\n");
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
            return this.Equals(input as PremiumAccountQuery);
        }

        /// <summary>
        /// Returns true if PremiumAccountQuery instances are equal
        /// </summary>
        /// <param name="input">Instance of PremiumAccountQuery to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PremiumAccountQuery input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.DateFrom == input.DateFrom ||
                    (this.DateFrom != null &&
                    this.DateFrom.Equals(input.DateFrom))
                ) && 
                (
                    this.DateTo == input.DateTo ||
                    (this.DateTo != null &&
                    this.DateTo.Equals(input.DateTo))
                ) && 
                (
                    this.Country == input.Country ||
                    this.Country.Equals(input.Country)
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
                if (this.DateFrom != null)
                {
                    hashCode = (hashCode * 59) + this.DateFrom.GetHashCode();
                }
                if (this.DateTo != null)
                {
                    hashCode = (hashCode * 59) + this.DateTo.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.Country.GetHashCode();
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