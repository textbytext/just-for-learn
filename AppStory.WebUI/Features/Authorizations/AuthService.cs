using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AppStory.Authorizations
{
	public class AuthService
	{
		string _secret = "mjgo3j5600-u)io)#$(5UI0JF30";

		public string[] GetRolesByPolicyName(string policyName)
		{
			return policyName switch
			{
				"GetAllProducts" => new string[] { "Customer" },
				_ => null,
			};
		}

		public string GenerateJwtToken(JwtDto data)
		{
			var validPeriod = TimeSpan.FromSeconds(60 * 60 * 24);
			var expirationTime = DateTime.UtcNow.Add(validPeriod);

			var jwtBuilder = new JwtBuilder()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(_secret)
				.ExpirationTime(expirationTime);

			jwtBuilder.AddClaim(ClaimTypes.UserData, data.ToJson());

			return jwtBuilder.Encode();
		}

		public JwtDto DecodeJwtToken(string jwt)
		{
			return new JwtBuilder()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(_secret)
				.MustVerifySignature()
				.Decode<Dictionary<string, string>>(jwt)
				.Single(p => p.Key == ClaimTypes.UserData)
				.Value
				.FromJson<JwtDto>();
		}
	}
}
