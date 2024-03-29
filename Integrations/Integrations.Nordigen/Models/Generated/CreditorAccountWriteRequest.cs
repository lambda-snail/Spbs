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
    /// Creditor account write serializer.
    /// </summary>
    [DataContract(Name = "CreditorAccountWriteRequest")]
    public partial class CreditorAccountWriteRequest : IEquatable<CreditorAccountWriteRequest>, IValidatableObject
    {

        /// <summary>
        /// Creditor account type
        /// </summary>
        /// <value>Creditor account type</value>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public TypeEnum? Type { get; set; }

        /// <summary>
        /// Gets or Sets AddressCountry
        /// </summary>
        [DataMember(Name = "address_country", EmitDefaultValue = false)]
        public AddressCountryEnum? AddressCountry { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreditorAccountWriteRequest" /> class.
        /// </summary>
        [JsonConstructor]
        protected CreditorAccountWriteRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreditorAccountWriteRequest" /> class.
        /// </summary>
        /// <param name="name">Creditor account name (required).</param>
        /// <param name="type">Creditor account type.</param>
        /// <param name="account">Creditor account type identifier (required).</param>
        /// <param name="currency">Creditor account currency (required).</param>
        /// <param name="addressCountry">addressCountry.</param>
        /// <param name="institutionId">an Institution ID for this CreditorAccount.</param>
        /// <param name="agent">Creditor account BICFI Identifier.</param>
        /// <param name="agentName">Creditor account agent name.</param>
        /// <param name="addressStreet">Creditor account address street.</param>
        /// <param name="postCode">Creditor account address post code.</param>
        public CreditorAccountWriteRequest(string name = default(string), TypeEnum? type = default(TypeEnum?), string account = default(string), string currency = default(string), AddressCountryEnum? addressCountry = default(AddressCountryEnum?), string institutionId = default(string), string agent = default(string), string agentName = default(string), string addressStreet = default(string), string postCode = default(string))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new ArgumentNullException("name is a required property for CreditorAccountWriteRequest and cannot be null");
            }
            this.Name = name;
            // to ensure "account" is required (not null)
            if (account == null)
            {
                throw new ArgumentNullException("account is a required property for CreditorAccountWriteRequest and cannot be null");
            }
            this.Account = account;
            // to ensure "currency" is required (not null)
            if (currency == null)
            {
                throw new ArgumentNullException("currency is a required property for CreditorAccountWriteRequest and cannot be null");
            }
            this.Currency = currency;
            this.Type = type;
            this.AddressCountry = addressCountry;
            this.InstitutionId = institutionId;
            this.Agent = agent;
            this.AgentName = agentName;
            this.AddressStreet = addressStreet;
            this.PostCode = postCode;
        }

        /// <summary>
        /// Creditor account name
        /// </summary>
        /// <value>Creditor account name</value>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
        public string Name { get; set; }

        /// <summary>
        /// Creditor account type identifier
        /// </summary>
        /// <value>Creditor account type identifier</value>
        [DataMember(Name = "account", IsRequired = true, EmitDefaultValue = true)]
        public string Account { get; set; }

        /// <summary>
        /// Creditor account currency
        /// </summary>
        /// <value>Creditor account currency</value>
        [DataMember(Name = "currency", IsRequired = true, EmitDefaultValue = true)]
        public string Currency { get; set; }

        /// <summary>
        /// an Institution ID for this CreditorAccount
        /// </summary>
        /// <value>an Institution ID for this CreditorAccount</value>
        [DataMember(Name = "institution_id", EmitDefaultValue = false)]
        public string InstitutionId { get; set; }

        /// <summary>
        /// Creditor account BICFI Identifier
        /// </summary>
        /// <value>Creditor account BICFI Identifier</value>
        [DataMember(Name = "agent", EmitDefaultValue = false)]
        public string Agent { get; set; }

        /// <summary>
        /// Creditor account agent name
        /// </summary>
        /// <value>Creditor account agent name</value>
        [DataMember(Name = "agent_name", EmitDefaultValue = false)]
        public string AgentName { get; set; }

        /// <summary>
        /// Creditor account address street
        /// </summary>
        /// <value>Creditor account address street</value>
        [DataMember(Name = "address_street", EmitDefaultValue = false)]
        public string AddressStreet { get; set; }

        /// <summary>
        /// Creditor account address post code
        /// </summary>
        /// <value>Creditor account address post code</value>
        [DataMember(Name = "post_code", EmitDefaultValue = false)]
        public string PostCode { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreditorAccountWriteRequest {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Account: ").Append(Account).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  AddressCountry: ").Append(AddressCountry).Append("\n");
            sb.Append("  InstitutionId: ").Append(InstitutionId).Append("\n");
            sb.Append("  Agent: ").Append(Agent).Append("\n");
            sb.Append("  AgentName: ").Append(AgentName).Append("\n");
            sb.Append("  AddressStreet: ").Append(AddressStreet).Append("\n");
            sb.Append("  PostCode: ").Append(PostCode).Append("\n");
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
            return this.Equals(input as CreditorAccountWriteRequest);
        }

        /// <summary>
        /// Returns true if CreditorAccountWriteRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of CreditorAccountWriteRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CreditorAccountWriteRequest input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.Type == input.Type ||
                    this.Type.Equals(input.Type)
                ) && 
                (
                    this.Account == input.Account ||
                    (this.Account != null &&
                    this.Account.Equals(input.Account))
                ) && 
                (
                    this.Currency == input.Currency ||
                    (this.Currency != null &&
                    this.Currency.Equals(input.Currency))
                ) && 
                (
                    this.AddressCountry == input.AddressCountry ||
                    this.AddressCountry.Equals(input.AddressCountry)
                ) && 
                (
                    this.InstitutionId == input.InstitutionId ||
                    (this.InstitutionId != null &&
                    this.InstitutionId.Equals(input.InstitutionId))
                ) && 
                (
                    this.Agent == input.Agent ||
                    (this.Agent != null &&
                    this.Agent.Equals(input.Agent))
                ) && 
                (
                    this.AgentName == input.AgentName ||
                    (this.AgentName != null &&
                    this.AgentName.Equals(input.AgentName))
                ) && 
                (
                    this.AddressStreet == input.AddressStreet ||
                    (this.AddressStreet != null &&
                    this.AddressStreet.Equals(input.AddressStreet))
                ) && 
                (
                    this.PostCode == input.PostCode ||
                    (this.PostCode != null &&
                    this.PostCode.Equals(input.PostCode))
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
                if (this.Name != null)
                {
                    hashCode = (hashCode * 59) + this.Name.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.Type.GetHashCode();
                if (this.Account != null)
                {
                    hashCode = (hashCode * 59) + this.Account.GetHashCode();
                }
                if (this.Currency != null)
                {
                    hashCode = (hashCode * 59) + this.Currency.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.AddressCountry.GetHashCode();
                if (this.InstitutionId != null)
                {
                    hashCode = (hashCode * 59) + this.InstitutionId.GetHashCode();
                }
                if (this.Agent != null)
                {
                    hashCode = (hashCode * 59) + this.Agent.GetHashCode();
                }
                if (this.AgentName != null)
                {
                    hashCode = (hashCode * 59) + this.AgentName.GetHashCode();
                }
                if (this.AddressStreet != null)
                {
                    hashCode = (hashCode * 59) + this.AddressStreet.GetHashCode();
                }
                if (this.PostCode != null)
                {
                    hashCode = (hashCode * 59) + this.PostCode.GetHashCode();
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
            // Name (string) maxLength
            if (this.Name != null && this.Name.Length > 70)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Name, length must be less than 70.", new [] { "Name" });
            }

            // Account (string) maxLength
            if (this.Account != null && this.Account.Length > 128)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Account, length must be less than 128.", new [] { "Account" });
            }

            // Currency (string) maxLength
            if (this.Currency != null && this.Currency.Length > 3)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Currency, length must be less than 3.", new [] { "Currency" });
            }

            // Agent (string) maxLength
            if (this.Agent != null && this.Agent.Length > 128)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Agent, length must be less than 128.", new [] { "Agent" });
            }

            // AgentName (string) maxLength
            if (this.AgentName != null && this.AgentName.Length > 140)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for AgentName, length must be less than 140.", new [] { "AgentName" });
            }

            // AddressStreet (string) maxLength
            if (this.AddressStreet != null && this.AddressStreet.Length > 140)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for AddressStreet, length must be less than 140.", new [] { "AddressStreet" });
            }

            // PostCode (string) maxLength
            if (this.PostCode != null && this.PostCode.Length > 30)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for PostCode, length must be less than 30.", new [] { "PostCode" });
            }

            yield break;
        }
    }

}
