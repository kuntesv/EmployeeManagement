﻿using System.Data.SqlClient;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private EmployeeService _employeeService;


        // constructor created for initilizing the objects using DI.
        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;

        }

        /// <summary>
        /// This method will insert employee details into database.
        /// </summary>
        /// <param name="employeeDetails"></param>
        /// <returns></returns>
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDetails employeeDetails)
        {

            if (employeeDetails == null)
            {
                return BadRequest("Invalid input Employee details");
            }

            try
            {
                bool result = await _employeeService.AddEmployee(employeeDetails);

                if (result)
                {
                    return Ok("Employee with name " + employeeDetails.FirstName + " Added into database");
                }
                else
                {
                    return StatusCode(500, "Error occured");
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(500, "Error occured while processing request" + ex.Message);
            }
        }

        [HttpDelete("DeleteEmployeeById")]
        public async Task<IActionResult> DeleteEmployeeById([FromQuery] int empId)
        {
            if (empId < 1)
            {
                return BadRequest("Please enter valid input employee ID");
            }

            try
            {

                bool result = await _employeeService.DeleteEmployee(empId);

                if (result)
                {
                    return Ok($"Successfully Delete employee details with id {empId}");
                }
                else
                {
                    return NotFound($" Record not found for employee id : {empId}");

                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("GetEmployeeDetailsById")]
        public async Task<IActionResult> GetEmployeeDetailsById([FromQuery] int empId)
        {

            if (empId < 1)
            {
                return BadRequest("Enter valid Employee ID");
            }

            try
            {

               var result = await _employeeService.GetEmployeeDetailsById(empId);

                if (result != null)
                {
                    return Ok(result);

                }
                else
                {
                    return NotFound($"Details for employee {empId} not present");
                }

            }
            catch (Exception ex)
            {

                System.Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] int limit = 5, [FromQuery] int skip=0)
        {


            try
            {

                var result = await _employeeService.GetAllEmployeeDetails(limit , skip);

                if (result.Any())
                {
                    return Ok(result);

                }
                else
                {
                    return NotFound($"No Employee records present");
                }

            }
            catch (Exception ex)
            {

                System.Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }


        }

        [HttpPut("UpdateEmployeeById")]
        public async Task<IActionResult> UpdateEmployeeById([FromQuery] int EmployeeId , [FromBody] EmployeeDetails employeeDetails)
        {

            if(EmployeeId < 0 )
            {
                return BadRequest($"Invalid employee id provided {EmployeeId}");
            }

            try
            {


                bool result = await _employeeService.UpdateEmployeeDetails(EmployeeId, employeeDetails);

                if(result)
                {
                    return Ok($"Updated employee details for id {EmployeeId}");
                }
                else
                {
                    return NotFound($"Employee with ID {EmployeeId} not found");
                }

            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }



    }
}
