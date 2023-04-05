﻿using Answers.API.Helpers;
using Answers.API.Services;
using Answers.Shared.Entities;
using Answers.Shared.Enums;
using Answers.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Answers.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IApiService apiService, IUserHelper userHelper)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1037637599", "Jose", "Rendon", "jose@yopmail.com", "322 311 4620", "Calle falsa 123", UserType.Admin);
            await CheckUserAsync("1020304050", "Jeisson", "Garcia", "jeisson@yopmail.com", "322 300 2333", "Calle nula 123", UserType.Admin);
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
                if (city == null)
                {
                    city = await _context.Cities.FirstOrDefaultAsync();
                }

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = city,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                //_context.Countries.ExecuteDelete();

                var responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");
                if (!responseCountries.IsSuccess)
                    return;
                var lstResponseCountries = (List<CountryResponse>)responseCountries.Result!;

                var responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/states");
                if (!responseStates.IsSuccess)
                    return;
                var lstResponseStates = (List<StateResponse>)responseStates.Result!;

                //var lstTaskCities = lstResponseStates.Select(x =>
                //{
                //    var StateIso = x.Iso2;
                //    var CountryIso = lstResponseCountries.SingleOrDefault(c => c.Id == x.CountryId)?.Iso2;

                //    return Task.Run(async () =>
                //    {
                //        return new
                //        {
                //            StateIso,
                //            CountryIso,
                //            Cities = await GetCities(CountryIso: CountryIso!, StateIso: StateIso!)
                //        };
                //    });
                //});
                //var responseCities = await Task.WhenAll(lstTaskCities);

                var lstCountries = lstResponseCountries
                                .Select(country => new Country()
                                {
                                    Name = $"{country.Iso2} - {country.Name!}",
                                    States = lstResponseStates.Where(s => s.CountryId == country.Id)
                                             .Select(state => new State()
                                             {
                                                 Name = $"{state.Iso2} - {state.Name!}",
                                                 Cities = new List<City>() { new City() { Name = $"CITY OF COUNTRY: {country.Name} STATE: {state.Name}" } }
                                                 //Cities = responseCities.Where(cities => cities.CountryIso == country.Iso2 && cities.StateIso == state.Iso2).SelectMany(cities => cities.Cities.Select(city => new City { Name = city.Name!, })).ToList(),
                                             }).ToList()
                                });


                if (lstCountries.Any())
                {
                    _context.Countries.AddRange(lstCountries);
                    await _context.SaveChangesAsync();
                }
            }
        }
        private async Task<List<CityResponse>> GetCities(string CountryIso, string StateIso)
        {
            var responseStates = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{CountryIso}/states/{StateIso}/cities");

            if (!responseStates.IsSuccess)
                return await Task.FromResult(new List<CityResponse>());

            var response = (List<CityResponse>)responseStates.Result!;

            return response.ToList();
        }
        //private async Task CheckCountriesAsync()
        //{
        //    if (!_context.Countries.Any())
        //    {
        //        Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");
        //        if (responseCountries.IsSuccess)
        //        {
        //            List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result!;
        //            foreach (CountryResponse countryResponse in countries)
        //            {
        //                Country? country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
        //                if (country == null)
        //                {
        //                    country = new() { Name = countryResponse.Name!, States = new List<State>() };
        //                    Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
        //                    if (responseStates.IsSuccess)
        //                    {
        //                        List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
        //                        foreach (StateResponse stateResponse in states!)
        //                        {
        //                            State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
        //                            if (state == null)
        //                            {
        //                                state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
        //                                Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
        //                                if (responseCities.IsSuccess)
        //                                {
        //                                    List<CityResponse> cities = (List<CityResponse>)responseCities.Result!;
        //                                    foreach (CityResponse cityResponse in cities)
        //                                    {
        //                                        if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
        //                                        {
        //                                            continue;
        //                                        }
        //                                        City city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
        //                                        if (city == null)
        //                                        {
        //                                            state.Cities.Add(new City() { Name = cityResponse.Name! });
        //                                        }
        //                                    }
        //                                }
        //                                if (state.CitiesNumber > 0)
        //                                {
        //                                    country.States.Add(state);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (country.StatesNumber > 0)
        //                    {
        //                        _context.Countries.Add(country);
        //                        await _context.SaveChangesAsync();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}