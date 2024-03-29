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
    /// PaymentReadSerializer.
    /// </summary>
    [DataContract(Name = "PaymentRead")]
    public partial class PaymentRead : IEquatable<PaymentRead>, IValidatableObject
    {

        /// <summary>
        /// Payment end to end identification
        /// </summary>
        /// <value>Payment end to end identification</value>
        [DataMember(Name = "payment_status", EmitDefaultValue = false)]
        public PaymentStatusEnum? PaymentStatus { get; set; }

        /// <summary>
        /// Returns false as PaymentStatus should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializePaymentStatus()
        {
            return false;
        }

        /// <summary>
        /// Payment product
        /// </summary>
        /// <value>Payment product</value>
        [DataMember(Name = "payment_product", EmitDefaultValue = false)]
        public PaymentProductEnum? PaymentProduct { get; set; }

        /// <summary>
        /// Payment Type
        /// </summary>
        /// <value>Payment Type</value>
        [DataMember(Name = "payment_type", EmitDefaultValue = false)]
        public PaymentTypeEnum? PaymentType { get; set; }

        /// <summary>
        /// Returns false as PaymentType should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializePaymentType()
        {
            return false;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRead" /> class.
        /// </summary>
        [JsonConstructor]
        protected PaymentRead() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRead" /> class.
        /// </summary>
        /// <param name="paymentProduct">Payment product.</param>
        /// <param name="redirect">Redirect URL to your application after payment is done (required).</param>
        /// <param name="description">Payment description (default to &quot;GOCARDLESS&quot;).</param>
        /// <param name="customPaymentId">Payment Custom Payment ID.</param>
        /// <param name="creditorAccount">Registered creditor account (required).</param>
        /// <param name="debtorAccount">debtorAccount (required).</param>
        /// <param name="instructedAmount">instructedAmount (required).</param>
        public PaymentRead(PaymentProductEnum? paymentProduct = default(PaymentProductEnum?), string redirect = default(string), string description = "GOCARDLESS", string customPaymentId = default(string), Guid creditorAccount = default(Guid), DebtorAccountWrite debtorAccount = default(DebtorAccountWrite), PaymentReadInstructedAmount instructedAmount = default(PaymentReadInstructedAmount))
        {
            // to ensure "redirect" is required (not null)
            if (redirect == null)
            {
                throw new ArgumentNullException("redirect is a required property for PaymentRead and cannot be null");
            }
            this.Redirect = redirect;
            this.CreditorAccount = creditorAccount;
            // to ensure "debtorAccount" is required (not null)
            if (debtorAccount == null)
            {
                throw new ArgumentNullException("debtorAccount is a required property for PaymentRead and cannot be null");
            }
            this.DebtorAccount = debtorAccount;
            // to ensure "instructedAmount" is required (not null)
            if (instructedAmount == null)
            {
                throw new ArgumentNullException("instructedAmount is a required property for PaymentRead and cannot be null");
            }
            this.InstructedAmount = instructedAmount;
            this.PaymentProduct = paymentProduct;
            // use default value if no "description" provided
            this.Description = description ?? "GOCARDLESS";
            this.CustomPaymentId = customPaymentId;
        }

        /// <summary>
        /// Payment ID
        /// </summary>
        /// <value>Payment ID</value>
        [DataMember(Name = "payment_id", EmitDefaultValue = false)]
        public string PaymentId { get; private set; }

        /// <summary>
        /// Returns false as PaymentId should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializePaymentId()
        {
            return false;
        }
        /// <summary>
        /// Redirect URL to your application after payment is done
        /// </summary>
        /// <value>Redirect URL to your application after payment is done</value>
        [DataMember(Name = "redirect", IsRequired = true, EmitDefaultValue = true)]
        public string Redirect { get; set; }

        /// <summary>
        /// Payment description
        /// </summary>
        /// <value>Payment description</value>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Payment Custom Payment ID
        /// </summary>
        /// <value>Payment Custom Payment ID</value>
        [DataMember(Name = "custom_payment_id", EmitDefaultValue = false)]
        public string CustomPaymentId { get; set; }

        /// <summary>
        /// Registered creditor account
        /// </summary>
        /// <value>Registered creditor account</value>
        [DataMember(Name = "creditor_account", IsRequired = true, EmitDefaultValue = true)]
        public Guid CreditorAccount { get; set; }

        /// <summary>
        /// Gets or Sets DebtorAccount
        /// </summary>
        [DataMember(Name = "debtor_account", IsRequired = true, EmitDefaultValue = true)]
        public DebtorAccountWrite DebtorAccount { get; set; }

        /// <summary>
        /// Gets or Sets InstructedAmount
        /// </summary>
        [DataMember(Name = "instructed_amount", IsRequired = true, EmitDefaultValue = true)]
        public PaymentReadInstructedAmount InstructedAmount { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class PaymentRead {\n");
            sb.Append("  PaymentId: ").Append(PaymentId).Append("\n");
            sb.Append("  PaymentStatus: ").Append(PaymentStatus).Append("\n");
            sb.Append("  PaymentProduct: ").Append(PaymentProduct).Append("\n");
            sb.Append("  PaymentType: ").Append(PaymentType).Append("\n");
            sb.Append("  Redirect: ").Append(Redirect).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  CustomPaymentId: ").Append(CustomPaymentId).Append("\n");
            sb.Append("  CreditorAccount: ").Append(CreditorAccount).Append("\n");
            sb.Append("  DebtorAccount: ").Append(DebtorAccount).Append("\n");
            sb.Append("  InstructedAmount: ").Append(InstructedAmount).Append("\n");
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
            return this.Equals(input as PaymentRead);
        }

        /// <summary>
        /// Returns true if PaymentRead instances are equal
        /// </summary>
        /// <param name="input">Instance of PaymentRead to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PaymentRead input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.PaymentId == input.PaymentId ||
                    (this.PaymentId != null &&
                    this.PaymentId.Equals(input.PaymentId))
                ) && 
                (
                    this.PaymentStatus == input.PaymentStatus ||
                    this.PaymentStatus.Equals(input.PaymentStatus)
                ) && 
                (
                    this.PaymentProduct == input.PaymentProduct ||
                    this.PaymentProduct.Equals(input.PaymentProduct)
                ) && 
                (
                    this.PaymentType == input.PaymentType ||
                    this.PaymentType.Equals(input.PaymentType)
                ) && 
                (
                    this.Redirect == input.Redirect ||
                    (this.Redirect != null &&
                    this.Redirect.Equals(input.Redirect))
                ) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.CustomPaymentId == input.CustomPaymentId ||
                    (this.CustomPaymentId != null &&
                    this.CustomPaymentId.Equals(input.CustomPaymentId))
                ) && 
                (
                    this.CreditorAccount == input.CreditorAccount ||
                    (this.CreditorAccount != null &&
                    this.CreditorAccount.Equals(input.CreditorAccount))
                ) && 
                (
                    this.DebtorAccount == input.DebtorAccount ||
                    (this.DebtorAccount != null &&
                    this.DebtorAccount.Equals(input.DebtorAccount))
                ) && 
                (
                    this.InstructedAmount == input.InstructedAmount ||
                    (this.InstructedAmount != null &&
                    this.InstructedAmount.Equals(input.InstructedAmount))
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
                if (this.PaymentId != null)
                {
                    hashCode = (hashCode * 59) + this.PaymentId.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.PaymentStatus.GetHashCode();
                hashCode = (hashCode * 59) + this.PaymentProduct.GetHashCode();
                hashCode = (hashCode * 59) + this.PaymentType.GetHashCode();
                if (this.Redirect != null)
                {
                    hashCode = (hashCode * 59) + this.Redirect.GetHashCode();
                }
                if (this.Description != null)
                {
                    hashCode = (hashCode * 59) + this.Description.GetHashCode();
                }
                if (this.CustomPaymentId != null)
                {
                    hashCode = (hashCode * 59) + this.CustomPaymentId.GetHashCode();
                }
                if (this.CreditorAccount != null)
                {
                    hashCode = (hashCode * 59) + this.CreditorAccount.GetHashCode();
                }
                if (this.DebtorAccount != null)
                {
                    hashCode = (hashCode * 59) + this.DebtorAccount.GetHashCode();
                }
                if (this.InstructedAmount != null)
                {
                    hashCode = (hashCode * 59) + this.InstructedAmount.GetHashCode();
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
            // Redirect (string) maxLength
            if (this.Redirect != null && this.Redirect.Length > 1024)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Redirect, length must be less than 1024.", new [] { "Redirect" });
            }

            // CustomPaymentId (string) maxLength
            if (this.CustomPaymentId != null && this.CustomPaymentId.Length > 35)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for CustomPaymentId, length must be less than 35.", new [] { "CustomPaymentId" });
            }

            yield break;
        }
    }

}
