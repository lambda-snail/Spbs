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
using System.ComponentModel.DataAnnotations;

namespace Integrations.Nordigen.Models
{
    /// <summary>
    /// PaginatedPaymentReadList
    /// </summary>
    [DataContract(Name = "PaginatedPaymentReadList")]
    public partial class PaginatedPaymentReadList : IEquatable<PaginatedPaymentReadList>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedPaymentReadList" /> class.
        /// </summary>
        /// <param name="count">count.</param>
        /// <param name="next">next.</param>
        /// <param name="previous">previous.</param>
        /// <param name="results">results.</param>
        public PaginatedPaymentReadList(int count = default(int), string next = default(string), string previous = default(string), List<PaymentRead> results = default(List<PaymentRead>))
        {
            this.Count = count;
            this.Next = next;
            this.Previous = previous;
            this.Results = results;
        }

        /// <summary>
        /// Gets or Sets Count
        /// </summary>
        [DataMember(Name = "count", EmitDefaultValue = false)]
        public int Count { get; set; }

        /// <summary>
        /// Gets or Sets Next
        /// </summary>
        [DataMember(Name = "next", EmitDefaultValue = true)]
        public string Next { get; set; }

        /// <summary>
        /// Gets or Sets Previous
        /// </summary>
        [DataMember(Name = "previous", EmitDefaultValue = true)]
        public string Previous { get; set; }

        /// <summary>
        /// Gets or Sets Results
        /// </summary>
        [DataMember(Name = "results", EmitDefaultValue = false)]
        public List<PaymentRead> Results { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class PaginatedPaymentReadList {\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            sb.Append("  Next: ").Append(Next).Append("\n");
            sb.Append("  Previous: ").Append(Previous).Append("\n");
            sb.Append("  Results: ").Append(Results).Append("\n");
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
            return this.Equals(input as PaginatedPaymentReadList);
        }

        /// <summary>
        /// Returns true if PaginatedPaymentReadList instances are equal
        /// </summary>
        /// <param name="input">Instance of PaginatedPaymentReadList to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PaginatedPaymentReadList input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Count == input.Count ||
                    this.Count.Equals(input.Count)
                ) && 
                (
                    this.Next == input.Next ||
                    (this.Next != null &&
                    this.Next.Equals(input.Next))
                ) && 
                (
                    this.Previous == input.Previous ||
                    (this.Previous != null &&
                    this.Previous.Equals(input.Previous))
                ) && 
                (
                    this.Results == input.Results ||
                    this.Results != null &&
                    input.Results != null &&
                    this.Results.SequenceEqual(input.Results)
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
                hashCode = (hashCode * 59) + this.Count.GetHashCode();
                if (this.Next != null)
                {
                    hashCode = (hashCode * 59) + this.Next.GetHashCode();
                }
                if (this.Previous != null)
                {
                    hashCode = (hashCode * 59) + this.Previous.GetHashCode();
                }
                if (this.Results != null)
                {
                    hashCode = (hashCode * 59) + this.Results.GetHashCode();
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
