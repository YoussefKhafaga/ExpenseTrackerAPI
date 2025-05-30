using ExpenseTrackerAPI.DTOs.ExpenseDTOs;
using ExpenseTrackerAPI.Services.ExpenseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseServices _expenseServices;

        public ExpenseController(IExpenseServices expenseServices)
        {
            _expenseServices = expenseServices;
        }

        [HttpGet("Expenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllExpenses()
        {
            try
            {
                var expenses = await _expenseServices.GetAllExpensesAsync();
                return Ok(expenses);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to access this expense.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No expenses found for the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Expense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDTO expenseDto)
        {
            try
            {
                var createdExpense = await _expenseServices.CreateExpenseAsync(expenseDto);
                return CreatedAtAction(nameof(GetExpenseById), new { id = createdExpense.Id }, createdExpense);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You must Login to create an expense.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            try
            {
                var expense = await _expenseServices.GetExpenseByIdAsync(id);
                return Ok(expense);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to access this expense.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Expense with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] UpdateExpenseDTO expenseDto)
        {
            try
            {
                var updatedExpense = await _expenseServices.UpdateExpenseAsync(id, expenseDto);
                return Ok(updatedExpense);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to update this expense.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Expense with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                var isDeleted = await _expenseServices.DeleteExpenseAsync(id);
                if (isDeleted)
                {
                    return NoContent();
                }
                return NotFound($"Expense with ID {id} not found.");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to delete this expense.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
    
}
