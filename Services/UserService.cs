using Net_Test_1_V4___Jaza_Arif.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO.Packaging;
using System.Windows.Controls;

namespace Net_Test_1_V4___Jaza_Arif.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _accessToken; // Store your access token here

        public UserService()
        {
            _httpClient = new HttpClient();
            _accessToken = "0bf7fb56e6a27cbcadc402fc2fce8e3aa9ac2b40d4190698eb4e8df9284e2023";
            _apiBaseUrl = "https://gorest.co.in/public/v2/";
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
        public async Task<List<User>> GetUsersAsync(int PageNo, string Filter)
        {
            try
            {
                string filterforQuery = "";
                if (string.IsNullOrWhiteSpace(Filter))
                {
                    filterforQuery = "?" + "page=" + PageNo + "&" + Filter;
                }
                else
                {
                    filterforQuery = "?" + Filter;
                }

                var response = await _httpClient.GetAsync("users" + filterforQuery);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<User>>(jsonResponse);
                }
                else
                {
                    // Handle API error response
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return null;
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await _httpClient.PostAsync("users", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(jsonResponse);
                }
                else
                {
                    // Handle API error response
                    return null;
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions
                return null;
            }
        }


        public async Task<User> GetUserAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"users/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(jsonResponse);
                }
                else
                {
                    // Handle API error response
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return null;
            }
        }

        public async Task<User> UpdateUserAsync(int userId, User updatedUserData)
        {
            try
            {
                // Serialize the updated user data to JSON
                var json = JsonConvert.SerializeObject(updatedUserData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the PATCH request to the API endpoint for updating a specific user
                var response = await _httpClient.PatchAsync($"users/{userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(jsonResponse);
                }
                else
                {
                    // Handle API error response
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {

                var response = await _httpClient.DeleteAsync($"users/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    // User deletion was successful
                    return true;
                }
                else
                {
                    // Handle API error response
                    return false;
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions
                return false;
            }
        }
        // Implement similar methods for updating and deleting users
    }
}
